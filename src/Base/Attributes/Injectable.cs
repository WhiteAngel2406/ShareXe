namespace ShareXe.Base.Attributes
{
    /// <summary>
    /// Attribute to mark classes for automatic dependency injection registration.
    /// </summary>
    /// <param name="lifetime">The service lifetime for the registered service. Defaults to Scoped.</param>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped) : Attribute
    {
        /// <summary>
        /// The service lifetime for dependency injection.
        /// </summary>
        public ServiceLifetime Lifetime { get; } = lifetime;
    }
}
