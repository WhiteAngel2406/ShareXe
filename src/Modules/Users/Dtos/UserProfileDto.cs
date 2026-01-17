using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;
using ShareXe.Modules.Minio.Dtos;

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
