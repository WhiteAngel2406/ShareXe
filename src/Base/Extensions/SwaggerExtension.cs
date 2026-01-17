using System.Reflection;

using Microsoft.OpenApi.Models;

using ShareXe.Base.Dtos;

using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

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

                options.OperationFilter<SwaggerPatchFilter>();

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

                options.AddHeathCheckSwaggerEndpoint();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.DocumentFilter<TagOrderFilter>();
            });
            return services;
        }

        internal class TagOrderFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Tags = [.. swaggerDoc.Tags
                    .OrderBy(t => t.Name switch
                    {
                        "Infrastructure" => 1,
                        "Auth" => 2,
                        _ => 100
                    })
                    .ThenBy(t => t.Name)];
            }
        }

        internal class SwaggerPatchFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var patchParameter = context.ApiDescription.ParameterDescriptions
                    .FirstOrDefault(p => p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(PatchRequest<>));

                if (patchParameter != null)
                {
                    var dtoType = patchParameter.Type.GenericTypeArguments[0];

                    var schema = context.SchemaGenerator.GenerateSchema(dtoType, context.SchemaRepository);

                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = { ["application/json"] = new OpenApiMediaType { Schema = schema } }
                    };
                }
            }
        }
    }
}
