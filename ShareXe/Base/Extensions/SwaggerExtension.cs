using Microsoft.OpenApi.Models;
using ShareXe.Base.Repositories.Implements;
using ShareXe.Base.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Filters;
namespace ShareXe.Base.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            // Cấu hình Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShareXe API", Version = "v1" });

                // 1. Cấu hình "Cái khóa" (Bearer Token)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Nhập token vào ô bên dưới: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

               
                c.ExampleFilters();
            });

            // Đăng ký Example Provider từ thư viện Swashbuckle.AspNetCore.Filters
            services.AddSwaggerExamplesFromAssemblyOf<Program>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
