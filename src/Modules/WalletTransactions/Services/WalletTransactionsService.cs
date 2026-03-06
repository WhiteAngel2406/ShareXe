using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.WalletTransactions.Entities;
using ShareXe.src.Modules.Wallets.Repositories;
using ShareXe.src.Modules.Wallets.Services;
using ShareXe.src.Modules.WalletTransactions.Dtos;
using ShareXe.src.Modules.WalletTransactions.Repositories;

namespace ShareXe.src.Modules.WalletTransactions.Services
{
    [Injectable]
    public class WalletTransactionsService(
        WalletTransactionsRepository transactionRepository,
        WalletsRepository walletsRepository,
        WalletsService walletsService, // Dùng lại service ví để lấy ví hiện tại
        ShareXeDbContext context,      // Inject DbContext để dùng Transaction
        IMapper mapper)
    {
        public async Task<WalletTransactionDto> TopUpAsync(TopUpRequestDto request)
        {
            // 1. Lấy ví của user hiện tại (hàm này đã tự lo việc tạo ví nếu chưa có)
            var myWalletDto = await walletsService.GetMyWalletAsync();
            var wallet = await walletsRepository.GetByIdAsync(myWalletDto.Id);

            if (wallet == null) throw new AppException(ErrorCode.NotFound, "Không tìm thấy ví.");

            // 2. MỞ TRANSACTION DATABASE (Đảm bảo an toàn tuyệt đối)
            using var dbTransaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Bước A: Cộng tiền vào ví
                wallet.Balance += request.Amount;
                await walletsRepository.UpdateAsync(wallet);

                // Bước B: Lưu lịch sử giao dịch
                var transaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = request.Amount,
                    TransactionType = TransactionType.TopUp, 
                    PaymentMethod = request.PaymentMethod,
                    Description = $"Nạp {request.Amount:N0} VNĐ vào ví.",
                    Status = TransactionStatus.Complete, 
                    ReferenceCode = $"TU-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}" // Tạo mã ref ngẫu nhiên
                };
                await transactionRepository.AddAsync(transaction);

                // 3. COMMIT TRANSACTION (Chỉ khi A và B đều thành công)
                await dbTransaction.CommitAsync();

                return mapper.Map<WalletTransactionDto>(transaction);
            }
            catch (Exception)
            {
                // Nếu có bất kỳ lỗi gì ở A hoặc B, ROLLBACK toàn bộ, không có chuyện tiền bị trừ/cộng sai
                await dbTransaction.RollbackAsync();
                throw new AppException(ErrorCode.InvalidRequest, "Giao dịch thất bại. Vui lòng thử lại.");
            }
        }

        public async Task<IEnumerable<WalletTransactionDto>> GetMyTransactionsAsync()
        {
            var myWalletDto = await walletsService.GetMyWalletAsync();

            var queryOptions = new QueryOptions<WalletTransaction>
            {
                Filter = t => t.WalletId == myWalletDto.Id,
                // OrderBy = q => q.OrderByDescending(t => t.CreatedAt) // Sắp xếp mới nhất lên đầu
            };

            var transactions = await transactionRepository.GetAllAsync(queryOptions);
            return mapper.Map<IEnumerable<WalletTransactionDto>>(transactions);
        }
    }
}
