using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;

namespace ShareXe.Modules.App.Services
{
  [Injectable]
  public class AppService
  {
    public string GetHealth()
    {
      throw new AppException(ErrorCode.OperationNotAllowed, "Health check is currently disabled.");
      // return "Healthy";
    }
  }
}