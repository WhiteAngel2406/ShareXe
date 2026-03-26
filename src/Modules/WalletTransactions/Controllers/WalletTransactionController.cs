using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.WalletTransactions.Dtos;
using ShareXe.Modules.WalletTransactions.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.WalletTransactions.Controllers
{
    [ApiController]
    [Route("/api/v1/wallet-transactions")]
    [Produces("application/json")]
    [Authorize]
    public class WalletTransactionController(WalletTransactionsService transactionService) : ControllerBase
    {
        [HttpPost("top-up")]
        [SwaggerOperation(
            Summary = "Top up wallet balance",
            Description = "Simulates a top-up transaction. Adds the specified amount to the current user's wallet."
        )]
        [SwaggerResponse(200, "Returns the completed transaction details.")]
        public async Task<ActionResult<SuccessResponse<WalletTransactionDto>>> TopUp([FromBody] TopUpRequestDto request)
        {
            var transaction = await transactionService.TopUpAsync(request);
            return Ok(SuccessResponse<WalletTransactionDto>.WithData(transaction, "Nạp tiền thành công."));
        }

        [HttpGet("history")]
        [SwaggerOperation(
            Summary = "Get transaction history",
            Description = "Retrieves the list of wallet transactions for the current user."
        )]
        [SwaggerResponse(200, "Returns the transaction history.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WalletTransactionDto>>>> GetHistory()
        {
            var history = await transactionService.GetMyTransactionsAsync();
            return Ok(SuccessResponse<IEnumerable<WalletTransactionDto>>.WithData(history, "Lấy lịch sử giao dịch thành công."));
        }
    }
}
