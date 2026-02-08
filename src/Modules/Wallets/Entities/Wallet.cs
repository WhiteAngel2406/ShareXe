using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.Modules.WalletTransactions.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Wallets.Entities
{
    [Entity("wallets")]
    public class Wallet : BaseEntity
    {
        [Required]
        [Unique]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public string Currency { get; set; } = null!;

        public ICollection<WalletTransaction> Transactions { get; set; } = [];
    }
}
