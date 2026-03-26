using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.Wallets.Dtos;
using ShareXe.Modules.Wallets.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Wallets.Controllers
{
    [ApiController]
    [Route("/api/v1/wallets")]
    [Produces("application/json")]
    [Authorize] // Bắt buộc phải đăng nhập mới xem được ví
    public class WalletController(WalletsService walletsService) : ControllerBase
    {
        [HttpGet("my-wallet")]
        [SwaggerOperation(
            Summary = "Get my wallet",
            Description = "Retrieves the current user's wallet balance. If the wallet does not exist, it automatically creates an empty one."
        )]
        [SwaggerResponse(200, "Returns the wallet details.")]
        public async Task<ActionResult<SuccessResponse<WalletDto>>> GetMyWallet()
        {
            var wallet = await walletsService.GetMyWalletAsync();
            return Ok(SuccessResponse<WalletDto>.WithData(wallet, "Wallet retrieved successfully."));
        }
    }
}
