using System.Reflection;

using ShareXe.Base.Attributes;

namespace ShareXe.Base.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddAutoInject(this IServiceCollection services)
        {
            services.Scan(scan => scan
              .FromAssemblies(Assembly.GetExecutingAssembly())
              .AddClasses(classes => classes.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Scoped))
                .AsImplementedInterfaces().AsSelf().WithScopedLifetime()

              .AddClasses(classes => classes.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Transient))
                .AsImplementedInterfaces().AsSelf().WithTransientLifetime()

              .AddClasses(classes => classes.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Singleton))
                .AsImplementedInterfaces().AsSelf().WithSingletonLifetime()
            );

            Console.WriteLine("======= [DI AUTO-REGISTER LOG] =======");

            var injectableTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null);

            foreach (var type in injectableTypes)
            {
                var attr = type.GetCustomAttribute<InjectableAttribute>();

                var isRegistered = services.Any(sd => sd.ImplementationType == type || sd.ServiceType == type);

                if (isRegistered)
                {
                    Console.WriteLine($"[REGISTERED] | {attr!.Lifetime,-10} | {type.Name}");
                }
                else
                {
                    Console.WriteLine($"[FAILED]     | {attr!.Lifetime,-10} | {type.Name} (Check Scrutor filter)");
                }
            }
            Console.WriteLine("======================================");
        }
    }
}
