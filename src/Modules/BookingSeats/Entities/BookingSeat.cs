using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Modules.Bookings.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareXe.src.Modules.BookingSeats.Entities
{
    [Entity("booking_seats")]
    public class BookingSeat : BaseEntity
    {
        [Required]
        public Guid BookingId { get; set; }
        [Required]
        public String SeatCode { get; set; }
        public decimal SeatPrice { get; set; }
        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; }
    }
}
