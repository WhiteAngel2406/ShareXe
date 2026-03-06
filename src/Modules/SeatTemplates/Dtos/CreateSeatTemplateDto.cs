using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.SeatTemplates.Dtos
{
    public class CreateSeatTemplateDto
    {
        [Required(ErrorMessage = "Vui lòng chỉ định loại xe.")]
        public Guid VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Mã ghế không được để trống.")]
        [MaxLength(10)]
        public string SeatCode { get; set; } = null!;

        [Required]
        public int PositionX { get; set; }

        [Required]
        public int PositionY { get; set; }

        public bool IsDriverSeat { get; set; }
    }
}
