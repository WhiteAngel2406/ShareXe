using ShareXe.Base.Entities;

namespace ShareXe.src.Modules.BookingSeat.Entities
{
    public class BookingSeat : BaseEntity
    {
        public Guid BookingId { get; set; }
        public String SeatCode { get; set; }
        public decimal SeatPrice { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = []; 
    }
}
