using System.Security.Claims;
using ShareXe.Base.Attributes;

namespace ShareXe.Modules.Auth
{
  [Injectable]
  public class UserContext(IHttpContextAccessor httpContextAccessor)
  {
    public string? FirebaseUid
      => httpContextAccessor.HttpContext?.User.FindFirst("user_id")?.Value;

    public string? Email
      => httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;

    public string? PhoneNumber
      => httpContextAccessor.HttpContext?.User.FindFirst("phone_number")?.Value;

    public string? FullName
      => httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;

    public string? Avatar
      => httpContextAccessor.HttpContext?.User.FindFirst("picture")?.Value;

  }
}