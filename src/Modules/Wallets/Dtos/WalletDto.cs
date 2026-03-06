using ShareXe.Base.Dtos;

namespace ShareXe.src.Modules.Wallets.Dtos
{
    public class WalletDto : EntityDto
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = null!;
    }
}
