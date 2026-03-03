using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.VehicleTypes.Dtos
{
    public class CreateVehicleTypeDto
    {
        [Required(ErrorMessage = "Tên loại xe không được để trống")]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(1, 60, ErrorMessage = "Số ghế phải nằm trong khoảng từ 1 đến 60")]
        public int TotalSeat { get; set; }
    }
}
