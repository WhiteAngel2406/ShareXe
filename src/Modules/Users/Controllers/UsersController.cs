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

        [Authorize]
        [HttpPatch("me")]
        [SwaggerOperation(
          Summary = "Update current user profile",
          Description = "Updates the profile information of the currently authenticated user."
        )]
        [SwaggerResponse(200, "Returns the updated user's profile information.")]
        [SwaggerResponse(400, "Bad Request - Invalid input data")]
        public async Task<ActionResult<SuccessResponse<UserProfileDto>>> UpdateCurrentUserProfile([FromBody] PatchRequest<UpdateUserProfileDto> patchRequest)
        {
            var updatedUser = await usersService.UpdateCurrentUserProfileAsync(patchRequest);
            var userProfileDto = await usersService.MapToUserProfileDtoAsync(updatedUser);
            return Ok(SuccessResponse<UserProfileDto>.WithData(userProfileDto, "User profile updated successfully."));
        }

        [Authorize]
        [HttpPatch("me/avatar")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
          Summary = "Update current user avatar",
          Description = "Updates the avatar of the currently authenticated user."
        )]
        [SwaggerResponse(200, "Returns the updated user's profile information with new avatar.")]
        [SwaggerResponse(400, "Bad Request - Invalid input data")]
        public async Task<ActionResult<SuccessResponse<UserProfileDto>>> UpdateCurrentUserAvatar([FromForm] UpdateAvatarDto updateAvatarDto)
        {
            var updatedUser = await usersService.UpdateCurrentUserAvatarAsync(updateAvatarDto);
            var userProfileDto = await usersService.MapToUserProfileDtoAsync(updatedUser);
            return Ok(SuccessResponse<UserProfileDto>.WithData(userProfileDto, "User avatar updated successfully."));
        }
    }
}
