using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Base.Enums;
using ShareXe.Modules.Wallets.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.WalletTransactions.Entities
{
    [Entity("wallet_transactions")]
    public class WalletTransaction : BaseEntity
    {
        [Required]
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public string? ReferenceCode { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }
    }
}
