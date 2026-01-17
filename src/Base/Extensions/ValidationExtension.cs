using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;

namespace ShareXe.Base.Extensions
{
    public static class ValidationExtension
    {
        public static IServiceCollection AddCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
              options.InvalidModelStateResponseFactory = actionContext =>
              {
                  var errors = actionContext.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors.Select(e => new ErrorResponse.ValidationError
                {
                    Field = JsonNamingPolicy.CamelCase.ConvertName(kvp.Key),
                    Message = e.ErrorMessage
                }))
                .ToList();

                  var response = ErrorResponse.WithValidationErrors(errors);
                  return new BadRequestObjectResult(response);
              }
            );

            return services;
        }
    }
}
