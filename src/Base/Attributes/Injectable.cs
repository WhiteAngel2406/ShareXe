namespace ShareXe.Base.Attributes
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped) : Attribute
  {
    public ServiceLifetime Lifetime { get; } = lifetime;
  }
}