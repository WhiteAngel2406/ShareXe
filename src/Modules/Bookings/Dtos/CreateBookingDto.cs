using ShareXe.Base.Enums;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Bookings.Dtos
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Vui lòng chọn chuyến đi.")]
        public Guid TripId { get; set; }

        public string? Note { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public PaymentMethod PaymentMethod { get; set; }

        // Danh sách ID các ghế mà khách hàng muốn đặt
        [Required]
        [MinLength(1, ErrorMessage = "Bạn phải chọn ít nhất 1 chỗ ngồi.")]
        public List<string> SeatCodes { get; set; } = new();
    }
}
