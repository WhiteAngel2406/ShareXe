using System.ComponentModel.DataAnnotations;

using ShareXe.Base.Attributes;

namespace ShareXe.Modules.Users.Dtos
{
    public class UpdateAvatarDto
    {
        [Required]
        [AllowedMimeTypes("image/*")]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        public IFormFile Avatar { get; set; } = default!;
    }
}
