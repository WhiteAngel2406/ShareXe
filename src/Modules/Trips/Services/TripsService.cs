using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Repositories;
using ShareXe.Modules.Auth;
using ShareXe.Modules.BookingSeats.Entities;
using ShareXe.Modules.SeatTemplates.Entities;
using ShareXe.Modules.Trips.Entities;
using ShareXe.src.Modules.BookingSeats.Repositories;
using ShareXe.src.Modules.DriverProfiles.Repositories;
using ShareXe.src.Modules.SeatTemplates.Repositories;
using ShareXe.src.Modules.Trips.Dtos;
using ShareXe.src.Modules.Trips.Repositories;
using ShareXe.src.Modules.Vehicles.Repositories;

namespace ShareXe.src.Modules.Trips.Services
{
    [Injectable]
    public class TripsService(
         TripsRepository tripsRepository,
         DriverProfilesRepository driverProfilesRepository,
         VehiclesRepository vehiclesRepository,
         SeatTemplatesRepository seatTemplatesRepository, 
        BookingSeatsRepository bookingSeatsRepository,   
         UserContext userContext,
         IMapper mapper)
    {
        // Hàm phụ trợ: Lấy ID của Driver Profile hiện tại (Giống bên VehiclesService)
        private async Task<Guid> GetCurrentDriverIdAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            if (string.IsNullOrEmpty(firebaseUid))
                throw new AppException(ErrorCode.Unauthorized, "No authenticated user.");

            var driverProfile = await driverProfilesRepository.GetOneAsync(dp => dp.User.Account.FirebaseUid == firebaseUid);

            if (driverProfile == null)
                throw new AppException(ErrorCode.InvalidRequest, "You must register as a driver before creating trips.");

            return driverProfile.Id;
        }

        public async Task<TripDto> CreateTripAsync(CreateTripDto createDto)
        {
            // 1. Lấy ID của tài xế đang thao tác
            var driverId = await GetCurrentDriverIdAsync();

            // 2. Validate Xe: Chắc chắn rằng chiếc xe này thuộc về tài xế đang đăng nhập
            var vehicle = await vehiclesRepository.GetOneAsync(v => v.Id == createDto.VehicleId && v.DriverId == driverId);
            if (vehicle == null)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Không tìm thấy phương tiện hoặc phương tiện này không thuộc quyền sở hữu của bạn.");
            }

            // 3. Validate Địa điểm: Điểm đón và điểm đến không được giống nhau
            if (createDto.StartHubId == createDto.EndHubId)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Điểm đón và điểm đến không được trùng nhau.");
            }

            // 4. Validate Thời gian: Khởi hành phải trước lúc đến nơi
            if (createDto.DepartureTime >= createDto.EstimatedArrivalTime)
            {
                throw new AppException(ErrorCode.InvalidRequest, "Thời gian khởi hành phải trước thời gian đến dự kiến.");
            }

            // 5. Map dữ liệu và gán các trường mặc định
            var trip = mapper.Map<Trip>(createDto);
            trip.DriverProfileId = driverId;

            // Lưu ý: Bạn hãy kiểm tra lại file enum TripStatus của bạn xem trạng thái khởi tạo là gì (Pending, Created, Published...)
            // Ở đây mình tạm để là giá trị mặc định đầu tiên của enum.
            trip.Status = TripStatus.Scheduled;

            // 6. Lưu vào DB
            await tripsRepository.AddAsync(trip);

            // 7. Trả về kết quả
            return mapper.Map<TripDto>(trip);
        }

        public async Task<IEnumerable<TripDto>> GetTripsAsync()
        {
            // Lấy danh sách các chuyến đi
            // Tạm thời lấy tất cả. Sau này bạn có thể bổ sung QueryOptions để khách hàng lọc theo điểm đi, điểm đến, ngày giờ...
            var trips = await tripsRepository.GetAllAsync(new QueryOptions<Trip>());

            return mapper.Map<IEnumerable<TripDto>>(trips);
        }
        public async Task<IEnumerable<TripSeatDto>> GetTripSeatMapAsync(Guid tripId)
        {
            // 1. Lấy thông tin chuyến đi (Kèm theo thông tin Xe để biết VehicleTypeId)
            // Giả định BaseRepository của bạn có thể include ("Vehicle")
            var trip = await tripsRepository.GetOneAsync(
                t => t.Id == tripId,
                includeProperties: "Vehicle"
            );

            if (trip == null)
            {
                throw new AppException(ErrorCode.NotFound, "Chuyến đi không tồn tại.");
            }

            // 2. Lấy sơ đồ ghế gốc của loại xe này (Ví dụ: Lấy 16 ghế của Ford Transit)
            var seatTemplates = await seatTemplatesRepository.GetAllAsync(
                new QueryOptions<SeatTemplate> { Filter = st => st.VehicleTypeId == trip.Vehicle.TypeId }
            );

            // 3. Lấy danh sách các mã ghế ĐÃ BỊ ĐẶT trên chuyến đi này
            // (Chỉ lấy những ghế của các Booking KHÔNG bị Cancelled)
            var bookedSeats = await bookingSeatsRepository.GetAllAsync(
                new QueryOptions<BookingSeat>
                {
                    Filter = bs => bs.Booking.TripId == tripId && bs.Booking.Status != BookingStatus.Cancelled
                }
            );

            // Tạo một HashSet chứa mã ghế đã đặt để tìm kiếm cho nhanh
            var bookedSeatCodes = bookedSeats.Select(bs => bs.SeatCode).ToHashSet();

            // 4. Map dữ liệu để trả về sơ đồ hoàn chỉnh cho Front-end
            var tripSeatMap = seatTemplates.Select(st => new TripSeatDto
            {
                SeatCode = st.SeatCode,
                PositionX = st.PositionX,
                PositionY = st.PositionY,
                IsDriverSeat = st.IsDriverSeat,
                Price = trip.PricePerSeat, // Lấy giá từ bảng Trip
                // Ghế sẽ Available (trống) nếu KHÔNG PHẢI ghế tài xế VÀ CHƯA BỊ ai đặt
                IsAvailable = !st.IsDriverSeat && !bookedSeatCodes.Contains(st.SeatCode)
            }).ToList();

            return tripSeatMap;
        }
    }
}
