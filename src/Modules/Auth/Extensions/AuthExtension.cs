using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ShareXe.Modules.Auth.Extensions
{
  public static class AuthExtension
  {
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
    {
      var client = new HttpClient();
      var keys = client
          .GetStringAsync(
              "https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com")
          .Result;
      var originalKeys = new JsonWebKeySet(keys).GetSigningKeys();
      var additionalkeys = client
          .GetStringAsync(
              "https://www.googleapis.com/service_accounts/v1/jwk/securetoken@system.gserviceaccount.com")
          .Result;
      var morekeys = new JsonWebKeySet(additionalkeys).GetSigningKeys();
      var totalkeys = originalKeys.Concat(morekeys);

      var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
      var authority = $"https://securetoken.google.com/{projectId}";

      JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.IncludeErrorDetails = true;
          options.Authority = authority;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = true,
            ValidAudience = projectId,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = totalkeys
          };
          options.Events = new JwtBearerEvents
          {
            OnAuthenticationFailed = context =>
            {
              Console.WriteLine("Authentication failed: " + context.Exception.Message);
              return Task.CompletedTask;
            },
            OnChallenge = async context =>
            {
              context.HandleResponse();
              context.Response.StatusCode = 401;
              context.Response.ContentType = "application/json";
              var result = JsonSerializer.Serialize(new { error = "You are not authorized" });
              await context.Response.WriteAsync(result);
            }
          };
        });

      return services;
    }
  }
}