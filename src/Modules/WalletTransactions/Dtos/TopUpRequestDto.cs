using ShareXe.Base.Enums;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.WalletTransactions.Dtos
{
    public class TopUpRequestDto
    {
        [Required]
        [Range(10000, 100000000, ErrorMessage = "Số tiền nạp phải từ 10.000 đến 100.000.000 VNĐ.")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
