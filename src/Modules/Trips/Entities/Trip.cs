using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Base.Enums;
using ShareXe.src.Modules.VehicleTypes.Entities;

namespace ShareXe.src.Modules.Trips.Entities
{
    [Entity("trips")]
    public class Trip : BaseEntity
    {
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public VehicleType Vehicle { get; set; }
        public decimal TotalPrice { get; set; }
        public TripStatus Status { get; set; }
        public DateTimeOffset DepartureTime { get; set; }
        public DateTimeOffset EstimatedArrivalTime { get; set; }
        public decimal PricePerSeat { get; set; }
        
    }
}
