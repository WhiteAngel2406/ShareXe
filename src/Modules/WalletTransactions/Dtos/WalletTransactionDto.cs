using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;

namespace ShareXe.Modules.WalletTransactions.Dtos
{
    public class WalletTransactionDto : EntityDto
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Description { get; set; } = null!;
        public string? ReferenceCode { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } // Kế thừa từ BaseEntity để hiển thị ngày giờ
    }
}
