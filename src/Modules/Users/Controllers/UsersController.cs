using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareXe.Base.Dtos;
using ShareXe.Modules.Users.Dtos;
using ShareXe.Modules.Users.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Users.Controllers
{
  [ApiController]
  [Route("/api/v1/users/")]
  [Produces("application/json")]
  public class UsersController(UsersService usersService) : ControllerBase
  {
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(
      Summary = "Get current user",
      Description = "Retrieves the profile information of the currently authenticated user."
    )]
    [SwaggerResponse(200, "Returns the current user's profile information.")]
    [SwaggerResponse(401, "Unauthorized - User is not authenticated")]
    public async Task<ActionResult<SuccessResponse<UserProfileDto>>> GetCurrentUser()
    {
      var user = await usersService.GetCurrentUserAsync();
      var userProfileDto = await usersService.MapToUserProfileDtoAsync(user);
      return Ok(SuccessResponse<UserProfileDto>.WithData(userProfileDto, "Current user retrieved successfully."));
    }
  }
}