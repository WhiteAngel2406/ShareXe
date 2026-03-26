using ShareXe.Base.Attributes;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Vehicles.Dtos
{
    public class CreateVehicleDto
    {
        [Required]
        public Guid TypeId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Color { get; set; } = null!;

        [Required]
        [AllowedMimeTypes("image/*")] // Tận dụng lại Custom Attribute cực hay của bạn
        [MaxFileSize(5 * 1024 * 1024)] // Tối đa 5MB
        public IFormFile Image { get; set; } = default!;
    }
}
