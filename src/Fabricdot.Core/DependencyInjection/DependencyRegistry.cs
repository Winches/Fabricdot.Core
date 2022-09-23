using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

public class DependencyRegistry : IDependencyRegistry
{
    public Type ImplementationType { get; }

    public ICollection<Type> ServiceTypes { get; }

    public ServiceLifetime ServiceLifetime { get; }

    public RegistrationBehavior RegisterBehavior { get; }

    public DependencyRegistry(
        Type implementationType,
        IEnumerable<Type> serviceTypes,
        ServiceLifetime serviceLifetime,
        RegistrationBehavior registerBehavior)
    {
        ImplementationType = Guard.Against.Null(implementationType, nameof(implementationType));
        ServiceTypes = Guard.Against.NullOrEmpty(serviceTypes, nameof(serviceTypes)).ToList();
        ServiceLifetime = Guard.Against.EnumOutOfRange(serviceLifetime, nameof(serviceLifetime));
        RegisterBehavior = Guard.Against.EnumOutOfRange(registerBehavior, nameof(registerBehavior));
    }

    public virtual ICollection<ServiceDescriptor> ToServiceDescriptors()
    {
        if (ServiceTypes.Count > 1 && (ServiceLifetime is ServiceLifetime.Singleton or ServiceLifetime.Scoped))
        {
            // Redirect service to self.
            ServiceTypes.AddIfNotContains(ImplementationType);
            return ServiceTypes.Select(v => v == ImplementationType
                ? ServiceDescriptor.Describe(v, ImplementationType, ServiceLifetime)
                : ServiceDescriptor.Describe(v, s => s.GetRequiredService(ImplementationType), ServiceLifetime))
                               .ToList();
        }

        return ServiceTypes.Select(v => ServiceDescriptor.Describe(v, ImplementationType, ServiceLifetime))
                           .ToList();
    }
}