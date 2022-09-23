using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

public interface IDependencyRegistry
{
    Type ImplementationType { get; }

    ICollection<Type> ServiceTypes { get; }

    ServiceLifetime ServiceLifetime { get; }

    RegistrationBehavior RegisterBehavior { get; }

    ICollection<ServiceDescriptor> ToServiceDescriptors();
}