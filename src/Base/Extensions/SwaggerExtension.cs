using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
namespace ShareXe.Base.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.DescribeAllParametersInCamelCase();

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ShareXe API", Version = "v1" });

                // Định nghĩa Bearer
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Định nghĩa oauth2 cho Login Form
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/api/v1/auth/login-swagger", UriKind.Relative),
                            Scopes = new Dictionary<string, string>()
                        }
                    }
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>(false, "Bearer");
                options.OperationFilter<SecurityRequirementsOperationFilter>(false, "oauth2");

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
            return services;
        }
    }
}
