using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;

namespace ShareXe.Modules.Trips.Dtos
{
    public class TripDto : EntityDto
    {
        public Guid DriverProfileId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid StartHubId { get; set; }
        public Guid EndHubId { get; set; }
        public TripStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset DepartureTime { get; set; }
        public DateTimeOffset EstimatedArrivalTime { get; set; }
        public decimal PricePerSeat { get; set; }
    }
}
