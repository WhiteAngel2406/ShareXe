using Microsoft.EntityFrameworkCore;
using ShareXe.Models;

namespace ShareXe.Base.Extensions
{
  public static class DatabaseExtension
  {
    public static IServiceCollection AddDatabaseConfig(this IServiceCollection services)
    {
      var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
      var dbName = Environment.GetEnvironmentVariable("DB_DATABASE_NAME");
      var dbUser = "sa";
      var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

      var connectionString = $"Server=localhost,{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True";

      services.AddDbContext<ShareXeDbContext>(options =>
          options.UseSqlServer(connectionString));

      return services;
    }

    public static void WaitForDatabase(this IHost host)
    {
      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;
      var logger = services.GetRequiredService<ILogger<Program>>();
      var dbContext = services.GetRequiredService<ShareXeDbContext>();

      Exception? exception = null;

      var retries = 5;
      while (retries > 0)
      {
        try
        {
          logger.LogInformation("Checking database connectivity...");
          if (dbContext.Database.CanConnect())
          {
            logger.LogInformation("Database is connected!");
            return;
          }
        }
        catch (Exception ex)
        {
          exception = ex;
        }

        logger.LogWarning("Database is not reachable. Retrying in 5 seconds...");
        retries--;
        Thread.Sleep(5000);
      }

      logger.LogCritical("Could not connect to the database after multiple attempts.");
      throw new Exception("Database connection failed.", exception);
    }
  }
}