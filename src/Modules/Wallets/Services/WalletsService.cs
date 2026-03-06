using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Modules.Auth;
using ShareXe.Modules.Users.Repositories;
using ShareXe.Modules.Wallets.Entities;
using ShareXe.src.Modules.Wallets.Dtos;
using ShareXe.src.Modules.Wallets.Repositories;

namespace ShareXe.src.Modules.Wallets.Services
{
    [Injectable]
    public class WalletsService(
         WalletsRepository walletsRepository,
         UsersRepository usersRepository,
         UserContext userContext,
         IMapper mapper)
    {
        // Hàm phụ trợ lấy ID người dùng hiện tại
        private async Task<Guid> GetCurrentUserIdAsync()
        {
            var firebaseUid = userContext.FirebaseUid;
            if (string.IsNullOrEmpty(firebaseUid))
                throw new AppException(ErrorCode.Unauthorized, "Vui lòng đăng nhập.");

            var user = await usersRepository.GetOneAsync(u => u.Account.FirebaseUid == firebaseUid);
            if (user == null)
                throw new AppException(ErrorCode.NotFound, "Không tìm thấy thông tin tài khoản.");

            return user.Id;
        }

        public async Task<WalletDto> GetMyWalletAsync()
        {
            var userId = await GetCurrentUserIdAsync();

            // Tìm ví của User này
            var wallet = await walletsRepository.GetOneAsync(w => w.UserId == userId);

            // Nếu chưa có ví, tự động tạo mới một ví trống
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = userId,
                    Balance = 0,
                    Currency = "VND" // Mặc định tiền tệ là VNĐ
                };

                await walletsRepository.AddAsync(wallet);
            }

            return mapper.Map<WalletDto>(wallet);
        }
    }
}
