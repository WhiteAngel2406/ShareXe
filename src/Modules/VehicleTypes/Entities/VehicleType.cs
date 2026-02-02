using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.src.Modules.SeatTemplates.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.VehicleTypes.Entities
{
    [Entity("vehicle_types")]
    public class VehicleType : BaseEntity
    {
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        public int TotalSeat { get; set; }
        public ICollection<SeatTemplate> VehicleTypes { get; set; }
    }
}
