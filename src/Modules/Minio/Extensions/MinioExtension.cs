using Minio;

namespace ShareXe.Modules.Minio.Extensions
{
  public static class MinioExtension
  {
    public static IServiceCollection AddMinioConfig(this IServiceCollection services)
    {
      var accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY") ?? "minioadmin";
      var secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY") ?? "minioadmin";
      var bucket = Environment.GetEnvironmentVariable("MINIO_BUCKET") ?? "sharexe_bucket";
      var port = Environment.GetEnvironmentVariable("MINIO_PORT") ?? "9000";
      var endpoint = $"localhost:{port}";

      services.AddMinio(configureClient => configureClient
          .WithEndpoint(endpoint)
          .WithCredentials(accessKey, secretKey)
          .WithSSL(false)
          .Build());

      return services;
    }
  }
}