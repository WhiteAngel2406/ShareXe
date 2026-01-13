using Microsoft.AspNetCore.Mvc;
using ShareXe.Modules.App.Services;

namespace ShareXe.Modules.App.Controllers
{
  [Route("api/v1/")]
  [ApiController]
  public class AppController(AppService appService) : ControllerBase
  {

    [HttpGet("health")]
    public IActionResult GetStatus()
    {
      return new OkObjectResult(new { status = appService.GetHealth() });
    }
  }
}