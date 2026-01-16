using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;
using ShareXe.Modules.Minio.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Users.Dtos
{
  public class UserProfileDto : EntityDto
  {
    public string? Email { get; set; }

    [SwaggerSchema("The role of the user")]
    public Role Role { get; set; }

    public string? FullName { get; set; }

    public MinioFileResponse? Avatar { get; set; }
  }
}