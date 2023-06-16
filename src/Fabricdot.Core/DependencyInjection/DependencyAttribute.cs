using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DependencyAttribute : Attribute
{
    public ServiceLifetime? Lifetime { get; }

    public RegistrationBehavior RegisterBehavior { get; set; } = RegistrationBehavior.Default;

    public DependencyAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public DependencyAttribute()
    {
    }
}
