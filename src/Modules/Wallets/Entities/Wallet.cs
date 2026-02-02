using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.src.Modules.WalletTransactions.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.Wallet.Entities
{
    [Entity("wallets")]
    public class Wallet : BaseEntity
    {
        [Required]
        public Guid WalletId { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public String Currency {  get; set;}
        public ICollection<WalletTransaction> Transactions { get; set; }
    }
}
