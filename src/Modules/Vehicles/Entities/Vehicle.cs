using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Modules.VehicleTypes.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.Vehicle.Entities
{
    [Entity("vehicles")]
    public class Vehicle : BaseEntity
    {
        [Required]
        [Unique]
        public Guid DriverId { get; set; }
        [Required]
        public VehicleType Type { get; set; }
        [Required]
        public String PlateNumber { get; set; }
        public String Model { get; set; }
        public String Color { get; set; }
        public String ImageURL { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
