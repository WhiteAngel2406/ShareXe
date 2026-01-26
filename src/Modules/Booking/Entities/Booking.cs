using ShareXe.src.Base.Enums;

namespace ShareXe.src.Modules.Booking.Entities
{
    public class Booking
    {
        public Guid TripId { get; set; }
        public Guid PassengerId { get; set; }
        public String? Note { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; }
    }
}
