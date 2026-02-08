using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.SeatTemplates.Entities
{
    [Entity("seat_templates")]
    public class SeatTemplate : BaseEntity
    {
        [Required]
        public Guid VehicleTypeId { get; set; }

        [Required]
        public string SeatCode { get; set; } = null!;

        [Required]
        public int PositionX { get; set; }

        [Required]
        public int PositionY { get; set; }

        [Required]
        public bool IsDriverSeat { get; set; }
    }
}
