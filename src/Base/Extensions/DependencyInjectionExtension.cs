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
    }
  }
}