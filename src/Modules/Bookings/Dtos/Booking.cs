using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;

namespace ShareXe.src.Modules.Bookings.Dtos
{
    public class BookingDto : EntityDto
    {
        public Guid TripId { get; set; }

        public Guid PassengerId { get; set; }

        public string? Note { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        // Sau này khi bạn hoàn thiện module BookingSeats, bạn có thể tạo thêm BookingSeatDto
        // và mở comment dòng dưới đây để trả về chi tiết từng ghế đã đặt
        // public List<BookingSeatDto> BookingSeats { get; set; } = new();
    }
}
