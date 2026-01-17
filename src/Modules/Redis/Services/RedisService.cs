using NRedisStack;
using NRedisStack.RedisStackCommands;

using ShareXe.Base.Attributes;

using StackExchange.Redis;

namespace ShareXe.Modules.Redis.Services
{
    [Injectable(ServiceLifetime.Singleton)]
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration config)
        {
            var port = config["REDIS_PORT"] ?? "6379";
            var password = config["REDIS_PASSWORD"];

            var options = ConfigurationOptions.Parse($"localhost:{port}");
            if (!string.IsNullOrEmpty(password)) options.Password = password;

            _redis = ConnectionMultiplexer.Connect(options);
        }

        public IDatabase GetDatabase(int db = -1) => _redis.GetDatabase(db);
        public IJsonCommands Json => GetDatabase().JSON();
        public ISearchCommands Search => GetDatabase().FT();
    }
}
