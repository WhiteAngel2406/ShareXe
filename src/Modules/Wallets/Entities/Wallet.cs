using ShareXe.Modules.Users.Entities;
using ShareXe.src.Modules.WalletTransactions.Entities;

namespace ShareXe.src.Modules.Wallet.Entities
{
    public class Wallet
    {
        public Guid WalletId { get; set; }
        public User User { get; set; }
        public decimal Balance { get; set; }
        public String Currency {  get; set;}
        public ICollection<WalletTransaction> Transactions { get; set; }
    }
}
