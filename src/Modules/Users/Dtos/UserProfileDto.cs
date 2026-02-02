using ShareXe.Base.Dtos;
using ShareXe.Modules.Minio.Dtos;
using ShareXe.src.Base.Enums;

namespace ShareXe.Modules.Users.Dtos
{
    public class UserProfileDto : EntityDto
    {
        public string? Email { get; set; }

        public Role Role { get; set; }

        public string? FullName { get; set; }

        public MinioFileResponse? Avatar { get; set; }
    }
}
