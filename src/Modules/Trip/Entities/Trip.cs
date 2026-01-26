using ShareXe.Base.Enums;

namespace ShareXe.src.Modules.Trip.Entities
{
    public class Trip
    {
        public Guid DriverId { get; set; }
        public DriverProfile Driver { get; set; }
        public Guid VehicleId { get; set; }
        public VehicleType Vehicle { get; set; }
        public decimal TotalPrice { get; set; }
        public TripStatus Status { get; set; }
        public DateTimeOffset DepartureTime { get; set; }
        public DateTimeOffset EstimatedArrivalTime { get; set; }
        public decimal PricePerSeat { get; set; }
        public ICollection<BookingSeat> Bookings { get; set; }
    }
}
