using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.DriverProfiles.Entities;
using ShareXe.Modules.VehicleTypes.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Vehicles.Entities
{
    [Entity("vehicles")]
    public class Vehicle : BaseEntity
    {
        [Required]
        public Guid DriverId { get; set; }
        public DriverProfile Driver { get; set; } = null!;

        [Required]
        public Guid TypeId { get; set; }

        public VehicleType Type { get; set; } = null!;

        [Required]
        public string PlateNumber { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;

        [Required]
        public string ImageURL { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }
    }
}
