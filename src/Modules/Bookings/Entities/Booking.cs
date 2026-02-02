using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Base.Enums;
using ShareXe.src.Modules.BookingSeats.Entities;
using ShareXe.src.Modules.Trips.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShareXe.src.Modules.Bookings.Entities
{
    [Entity("bookings")]
    public class Booking : BaseEntity
    {
        [Required]
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; }
        [Required]
        public Guid PassengerId { get; set; }
        public String? Note { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        [Required]
        public BookingStatus Status { get; set; }
        [InverseProperty(nameof(BookingSeat.Booking))]
        public ICollection<BookingSeat> BookingSeats { get; set;} = new List<BookingSeat>();
    }
}
