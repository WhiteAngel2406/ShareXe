using ShareXe.Base.Attributes;

namespace ShareXe.Modules.App.Services
{
  [Injectable]
  public class AppService
  {
    public string GetHealth()
    {
      return "Healthy";
    }
  }
}