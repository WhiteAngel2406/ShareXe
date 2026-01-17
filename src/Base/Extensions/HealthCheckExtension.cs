using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using ShareXe.Modules.Minio.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShareXe.Base.Extensions
{
  public static class HealthCheckExtension
  {
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
      var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
      var dbName = Environment.GetEnvironmentVariable("DB_DATABASE_NAME");
      var dbUser = "sa";
      var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
      var dbConnectionString = $"Server=localhost,{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True";

      var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
      var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");

      services.AddHealthChecks()
        .AddSqlServer(
          connectionString: dbConnectionString,
          name: "SQL Server",
          tags: ["db", "sql", "sqlserver"],
          healthQuery: "SELECT 1"
        )
        .AddRedis(
          redisConnectionString: !string.IsNullOrEmpty(redisPassword)
            ? $"localhost:{redisPort},password={redisPassword}"
            : $"localhost:{redisPort}",
          name: "Redis",
          tags: ["db", "redis"]
        ).AddCheck<MinioHealthCheck>(
          name: "Minio",
          tags: ["storage", "minio"]
        );

      return services;
    }

    public static void MapAppHealthChecks(this IEndpointRouteBuilder endpoints)
    {
      endpoints.MapHealthChecks("/health", new HealthCheckOptions
      {
        ResponseWriter = async (context, report) =>
        {
          context.Response.ContentType = "application/json";

          var process = System.Diagnostics.Process.GetCurrentProcess();
          var uptime = DateTime.Now - process.StartTime;
          var memoryUsage = $"{process.WorkingSet64 / 1024 / 1024} MB";

          var response = new
          {
            status = report.Status.ToString(),
            version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            serverTime = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            uptime = uptime.ToString(@"dd\.hh\:mm\:ss"),
            memoryUsage,
            checks = report.Entries.Select(x => new
            {
              component = x.Key,
              status = x.Value.Status.ToString(),
              description = x.Value.Description,
              duration = $"{x.Value.Duration.TotalMilliseconds:0.##} ms"
            }),
            totalDuration = $"{report.TotalDuration.TotalMilliseconds:0.##} ms"
          };

          await context.Response.WriteAsync(JsonSerializer.Serialize(response, serializerOptions));
        }
      });
    }

    public static void AddHeathCheckSwaggerEndpoint(this SwaggerGenOptions options)
      => options.DocumentFilter<HealthCheckSwaggerFilter>();


    private static readonly JsonSerializerOptions serializerOptions = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };

    internal class MinioHealthCheck(MinioService minioService) : IHealthCheck
    {
      public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
      {
        try
        {
          await minioService.GetBucketsAsync();
          return HealthCheckResult.Healthy("Minio is healthy.");
        }
        catch (Exception ex)
        {
          return HealthCheckResult.Unhealthy(ex.Message);
        }
      }
    }

    internal class HealthCheckSwaggerFilter : IDocumentFilter
    {
      public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
      {
        var pathItem = new OpenApiPathItem();
        var operation = new OpenApiOperation
        {
          Tags = [new OpenApiTag() { Name = "Infrastructure", Description = "Infrastructure related endpoints" }],
          Summary = "System health check",
          Description = "Returns the health status of the application and its dependencies.",
        };

        operation.Responses.Add("200", new OpenApiResponse { Description = "The system is healthy." });
        operation.Responses.Add("503", new OpenApiResponse { Description = "The system is unhealthy." });

        pathItem.AddOperation(OperationType.Get, operation);
        swaggerDoc.Paths.Add("/health", pathItem);
      }
    }
  }
}