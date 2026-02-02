using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.SeatTemplates.Entities
{
    [Entity("seat_templates")]
    public class SeatTemplate : BaseEntity
    {
        [Required]
        public Guid VehicleTypeId { get; set; }
        [Required]
        public String SeatCode { get; set; }
        [Required]
        public int PositionX { get; set; }
        [Required]
        public int PositionY { get; set; }
        [Required]
        public bool isDriverSeated { get; set; }
    }
}
