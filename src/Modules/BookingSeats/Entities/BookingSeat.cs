using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Bookings.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.BookingSeats.Entities
{
    [Entity("booking_seats")]
    public class BookingSeat : BaseEntity
    {
        [Required]
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        [Required]
        public string SeatCode { get; set; } = null!;

        [Required]
        public decimal SeatPrice { get; set; }

    }
}
