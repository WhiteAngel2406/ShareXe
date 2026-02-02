using ShareXe.src.Base.Enums;

namespace ShareXe.src.Modules.WalletTransactions.Entities
{
    public class WalletTransaction
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public String Description { get; set; }
        public String? ReferenceCode { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
