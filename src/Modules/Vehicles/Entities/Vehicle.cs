using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.DriverProfiles.Entities;
using ShareXe.Modules.VehicleTypes.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareXe.Modules.Vehicles.Entities
{
    [Entity("vehicles")]
    public class Vehicle : BaseEntity
    {
        [Required]
        public Guid DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public DriverProfile Driver { get; set; } = null!;

        [Required]
        public Guid TypeId { get; set; }

        public VehicleType Type { get; set; } = null!;

        [Required]
        [Unique]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Color { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Image { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }
    }
}
