using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Base.Enums;
using ShareXe.Modules.BookingSeats.Entities;
using ShareXe.Modules.Trips.Entities;
using ShareXe.Modules.Users.Entities;

using System.ComponentModel.DataAnnotations;


namespace ShareXe.Modules.Bookings.Entities
{
    [Entity("bookings")]
    public class Booking : BaseEntity
    {
        [Required]
        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        [Required]
        public Guid PassengerId { get; set; }
        public User Passenger { get; set; } = null!;

        public string? Note { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        public ICollection<BookingSeat> BookingSeats { get; set; } = [];
    }
}
