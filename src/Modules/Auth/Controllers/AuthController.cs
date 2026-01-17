using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.Auth.Services;
using ShareXe.Modules.Users.Dtos;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Auth.Controllers
{
    [ApiController]
    [Route("/api/v1/auth/")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("login-swagger")]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(
            Summary = "Login for Swagger UI",
            Description = "Authenticates a user with username and password and returns an access token for API testing in Swagger UI.\n\nFor uses in Swagger UI only, do not use this in production."
        )]
        [SwaggerResponse(200, "Login successful", typeof(object))]
        [SwaggerResponse(400, "Invalid credentials or bad request")]
        public async Task<IActionResult> LoginForSwagger([FromForm] string username, [FromForm] string password)
        {
            var idToken = await authService.SignInWithEmailPasswordAsync(username, password);
            return Ok(new
            {
                access_token = idToken,
                token_type = "Bearer",
                expires_in = 3600
            });
        }

        [Authorize]
        [HttpPost("sync")]
        [SwaggerOperation(
            Summary = "Sync account from Firebase",
            Description = "Synchronizes the authenticated user's account information from Firebase."
        )]
        [SwaggerResponse(200, "Account synced successfully")]
        [SwaggerResponse(401, "Unauthorized - valid authentication required")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<ActionResult<SuccessResponse<UserProfileDto>>> SyncAccount()
        {
            var account = await authService.SyncAccountFromFirebaseAsync();
            var userProfileDto = await authService.MapToUserProfileDtoAsync(account);
            return Ok(SuccessResponse<UserProfileDto>.WithData(userProfileDto, "Account synced successfully."));
        }
    }
}
