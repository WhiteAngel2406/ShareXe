using Microsoft.AspNetCore.Mvc;
using ShareXe.Modules.App.Services;

namespace ShareXe.Modules.App.Controllers
{
  [Route("api/v1/")]
  [ApiController]
  public class AppController : ControllerBase
  {
    private readonly AppService _appService;

    public AppController(AppService appService)
    {
      _appService = appService;
    }

    [HttpGet("health")]
    public IActionResult GetStatus()
    {
      return new OkObjectResult(new { status = _appService.GetHealth() });
    }
  }
}