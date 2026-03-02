using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Base.Enums;
using ShareXe.Modules.Bookings.Entities;
using ShareXe.Modules.DriverProfiles.Entities;
using ShareXe.Modules.Hubs.Entities;
using ShareXe.Modules.Vehicles.Entities;

namespace ShareXe.Modules.Trips.Entities
{
    [Entity("trips")]
    public class Trip : BaseEntity
    {
        [Required]
        public Guid DriverProfileId { get; set; }
        public DriverProfile DriverProfile { get; set; } = null!;

        [Required]
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        [Required]
        public Guid StartHubId { get; set; }

        [ForeignKey(nameof(StartHubId))]
        public Hub StartHub { get; set; } = null!;

        [Required]
        public Guid EndHubId { get; set; }

        [ForeignKey(nameof(EndHubId))]
        public Hub EndHub { get; set; } = null!;

        public TripStatus Status { get; set; }
        [MaxLength(500)]
        public string? Note { get; set; }

        public DateTimeOffset DepartureTime { get; set; }

        public DateTimeOffset EstimatedArrivalTime { get; set; }

        public decimal PricePerSeat { get; set; }

        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
