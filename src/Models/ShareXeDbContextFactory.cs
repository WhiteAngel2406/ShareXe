using DotNetEnv;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShareXe.Models
{
    public class ShareXeDbContextFactory : IDesignTimeDbContextFactory<ShareXeDbContext>
    {
        public ShareXeDbContext CreateDbContext(string[] args)
        {
            Env.TraversePath().Load();

            var builder = new DbContextOptionsBuilder<ShareXeDbContext>();

            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
            var dbName = Environment.GetEnvironmentVariable("DB_DATABASE_NAME");
            var dbUser = "sa";
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = $"Server=localhost,{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True";

            builder.UseSqlServer(connectionString).UseSnakeCaseNamingConvention();
            return new ShareXeDbContext(builder.Options);
        }
    }
}
