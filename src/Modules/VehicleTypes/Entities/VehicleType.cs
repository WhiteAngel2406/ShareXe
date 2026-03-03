using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.SeatTemplates.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.VehicleTypes.Entities
{
    [Entity("vehicle_types")]
    public class VehicleType : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;


        [Required]
        public int TotalSeat { get; set; }

        public ICollection<SeatTemplate> SeatTemplates { get; set; } = [];
    }
}
