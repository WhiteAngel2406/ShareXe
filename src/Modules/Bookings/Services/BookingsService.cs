using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Auth;
using ShareXe.Modules.Bookings.Entities;
using ShareXe.Modules.BookingSeats.Entities;
using ShareXe.Modules.Users.Repositories;
using ShareXe.Modules.WalletTransactions.Entities;
using ShareXe.src.Modules.Bookings.Dtos;
using ShareXe.src.Modules.Bookings.Repositories;
using ShareXe.src.Modules.Trips.Repositories;
using ShareXe.src.Modules.Wallets.Repositories;
using ShareXe.src.Modules.WalletTransactions.Repositories;

namespace ShareXe.src.Modules.Bookings.Services
{
    [Injectable]
    public class BookingsService(
         BookingsRepository bookingsRepository,
         TripsRepository tripsRepository,
         UsersRepository usersRepository,
         WalletsRepository walletsRepository,                   // Inject mới
         WalletTransactionsRepository walletTransactionsRepository, // Inject mới
         ShareXeDbContext context,
         UserContext userContext,
         IMapper mapper)
    {
        // Hàm phụ trợ: Lấy UserId của hành khách đang đăng nhập
        private async Task<Guid> GetCurrentPassengerIdAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            if (string.IsNullOrEmpty(firebaseUid))
                throw new AppException(ErrorCode.Unauthorized, "No authenticated user.");

            var user = await usersRepository.GetOneAsync(u => u.Account.FirebaseUid == firebaseUid);

            if (user == null)
                throw new AppException(ErrorCode.NotFound, "User account not found.");

            return user.Id;
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createDto)
        {
            // 1. Xác thực người dùng (Lấy ID hành khách)
            var passengerId = await GetCurrentPassengerIdAsync();

            // 2. Kiểm tra xem Chuyến đi (Trip) có tồn tại không
            var trip = await tripsRepository.GetOneAsync(t => t.Id == createDto.TripId);
            if (trip == null)
            {
                throw new AppException(ErrorCode.NotFound, "Chuyến đi không tồn tại hoặc đã bị hủy.");
            }

            // (Tùy chọn bổ sung) Không cho phép tài xế tự đặt chuyến đi của chính mình
            // if (trip.DriverProfile.UserId == passengerId) 
            //     throw new AppException(ErrorCode.BadRequest, "Bạn không thể tự đặt chuyến đi của chính mình.");

            // 3. Tính toán Tổng Tiền = Số lượng ghế x Giá mỗi ghế
            // TUYỆT ĐỐI KHÔNG nhận TotalPrice từ Client gửi lên để tránh hack giá.
            var numberOfSeats = createDto.SeatCodes.Count;
            var calculatedTotalPrice = trip.PricePerSeat * numberOfSeats;

            // 4. Map từ DTO sang Entity và gán các giá trị Server-side
            var booking = mapper.Map<Booking>(createDto);
            booking.PassengerId = passengerId;
            booking.TotalPrice = calculatedTotalPrice;
            booking.Status = BookingStatus.Pending; // Trạng thái mặc định chờ xác nhận/thanh toán

            // 5. Xử lý danh sách Ghế (BookingSeats)
            // Lặp qua từng SeatId khách hàng gửi lên để tạo các entity BookingSeat tương ứng
            foreach (var seatCode in createDto.SeatCodes)
            {
                booking.BookingSeats.Add(new BookingSeat
                {
                    SeatCode = seatCode,
                    SeatPrice = trip.PricePerSeat // Gán luôn giá của ghế bằng giá của chuyến đi
                });
            }
         

            // 6. Lưu vào Database
            await bookingsRepository.AddAsync(booking);

            // 7. Trả về kết quả cho Client
            return mapper.Map<BookingDto>(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetMyBookingsAsync()
        {
            // 1. Lấy ID của hành khách hiện tại
            var passengerId = await GetCurrentPassengerIdAsync();

            // 2. Dùng QueryOptions siêu xịn của bạn để lọc ra các booking của đúng user này
            var queryOptions = new QueryOptions<Booking>
            {
                Filter = b => b.PassengerId == passengerId,
                // Sắp xếp chuyến nào đặt gần nhất lên đầu tiên (Tùy thuộc bạn định nghĩa OrderBy thế nào)
            };

            var bookings = await bookingsRepository.GetAllAsync(queryOptions);

            // 3. Trả về danh sách
            return mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<BookingDto> PayBookingAsync(Guid bookingId)
        {
            // 1. Lấy ID người dùng hiện tại
            var passengerId = await GetCurrentPassengerIdAsync();

            // 2. Tìm đơn đặt xe (Chỉ lấy đơn của chính user này để bảo mật)
            var booking = await bookingsRepository.GetOneAsync(b => b.Id == bookingId && b.PassengerId == passengerId);
            if (booking == null)
            {
                throw new AppException(ErrorCode.NotFound, "Không tìm thấy đơn đặt xe.");
            }

            // 3. Kiểm tra trạng thái đơn hàng (Chỉ thanh toán nếu đơn đang ở trạng thái Pending)
            if (booking.Status != BookingStatus.Pending)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Đơn đặt xe này không ở trạng thái chờ thanh toán.");
            }

            // 4. Lấy Ví của khách hàng để kiểm tra số dư
            var wallet = await walletsRepository.GetOneAsync(w => w.UserId == passengerId);
            if (wallet == null || wallet.Balance < booking.TotalPrice)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Số dư trong ví không đủ để thanh toán. Vui lòng nạp thêm tiền.");
            }

            // 5. BẮT ĐẦU GIAO DỊCH DATABASE (TRANSACTION)
            using var dbTransaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Bước A: Trừ tiền trong ví
                wallet.Balance -= booking.TotalPrice;
                await walletsRepository.UpdateAsync(wallet);

                // Bước B: Lưu lại lịch sử giao dịch (Biên lai trừ tiền)
                var transaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = booking.TotalPrice, // Ghi nhận số tiền
                    TransactionType = TransactionType.Payment, // Giả định bạn có loại giao dịch Thanh Toán
                    PaymentMethod = PaymentMethod.Wallet, // Trả bằng Ví nội bộ
                    Description = $"Thanh toán cho chuyến đi có mã đơn: {bookingId}",
                    Status = TransactionStatus.Complete,
                    ReferenceCode = $"PAY-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}"
                };
                await walletTransactionsRepository.AddAsync(transaction);

                // Bước C: Cập nhật trạng thái Booking thành Confirmed (Đã xác nhận/Đã thanh toán)
                booking.Status = BookingStatus.Confirmed;
                await bookingsRepository.UpdateAsync(booking);

                // 6. XÁC NHẬN GIAO DỊCH (Nếu A, B, C đều không có lỗi)
                await dbTransaction.CommitAsync();

                return mapper.Map<BookingDto>(booking);
            }
            catch (Exception)
            {
                // Nếu có bất kỳ lỗi nào xảy ra, Hủy bỏ mọi thay đổi (Tiền không bị trừ, Booking giữ nguyên Pending)
                await dbTransaction.RollbackAsync();
                throw new AppException(ErrorCode.InvalidRequest, "Quá trình thanh toán gặp sự cố. Vui lòng thử lại.");
            }
        }
        public async Task<BookingDto> CancelBookingAsync(Guid bookingId)
        {
            // 1. Xác thực người dùng
            var passengerId = await GetCurrentPassengerIdAsync();

            // 2. Tìm đơn đặt xe
            var booking = await bookingsRepository.GetOneAsync(b => b.Id == bookingId && b.PassengerId == passengerId);
            if (booking == null)
            {
                throw new AppException(ErrorCode.NotFound, "Không tìm thấy đơn đặt xe.");
            }

            // 3. Kiểm tra xem đơn có được phép hủy không
            // Không thể hủy đơn đã hoàn thành hoặc đã bị hủy từ trước
            if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Không thể hủy đơn đặt xe ở trạng thái hiện tại.");
            }

            // Kiểm tra xem đơn này đã thanh toán chưa (để biết có cần hoàn tiền không)
            bool requiresRefund = booking.Status == BookingStatus.Confirmed;

            // 4. BẮT ĐẦU TRANSACTION DATABASE
            using var dbTransaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Bước A: Đổi trạng thái đơn thành Cancelled
                booking.Status = BookingStatus.Cancelled;
                await bookingsRepository.UpdateAsync(booking);

                // Bước B: Nếu đã thanh toán, tiến hành HOÀN TIỀN
                if (requiresRefund)
                {
                    var wallet = await walletsRepository.GetOneAsync(w => w.UserId == passengerId);
                    if (wallet == null) throw new AppException(ErrorCode.NotFound, "Không tìm thấy ví để hoàn tiền.");

                    // Cộng lại 100% tiền vào ví (Bạn có thể thêm logic tính % hoàn tiền tùy theo thời gian thực tế ở đây)
                    wallet.Balance += booking.TotalPrice;
                    await walletsRepository.UpdateAsync(wallet);

                    // Tạo lịch sử giao dịch Hoàn Tiền
                    var transaction = new WalletTransaction
                    {
                        WalletId = wallet.Id,
                        Amount = booking.TotalPrice,
                        TransactionType = TransactionType.Refund, // Giả định bạn có enum Refund
                        PaymentMethod = PaymentMethod.Wallet,
                        Description = $"Hoàn tiền do hủy chuyến đi mã: {bookingId}",
                        Status = TransactionStatus.Complete,
                        ReferenceCode = $"RF-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}"
                    };
                    await walletTransactionsRepository.AddAsync(transaction);
                }

                // 5. COMMIT TRANSACTION (Xác nhận thay đổi)
                await dbTransaction.CommitAsync();

                return mapper.Map<BookingDto>(booking);
            }
            catch (Exception)
            {
                // Nếu lỗi, rollback để đảm bảo trạng thái ghế và tiền trong ví không bị sai lệch
                await dbTransaction.RollbackAsync();
                throw new AppException(ErrorCode.InvalidRequest, "Quá trình hủy chuyến gặp sự cố. Vui lòng thử lại.");
            }
        }
    }
}

