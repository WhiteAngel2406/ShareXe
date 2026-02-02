using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Base.Enums;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.WalletTransactions.Entities
{
    [Entity("wallet_transactions")]
    public class WalletTransaction : BaseEntity
    {
        [Required]
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public String Description { get; set; }
        [Required]
        public String? ReferenceCode { get; set; }
        [Required]
        public TransactionStatus Status { get; set; }
    }
}
