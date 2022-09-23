using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Events;
using Fabricdot.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Domain.DependencyInjection;

public class DomainDependencyRegistrar : DefaultDependencyRegistrar
{
    protected static readonly Type DomainEventHanlderType = typeof(IDomainEventHandler<>);

    protected static readonly Type DomainServiceType = typeof(IDomainService);

    protected static readonly Type RepositoryType = typeof(IRepository);

    protected override bool CanRegister(Type typeToRegister)
    {
        return (typeToRegister.IsAssignableTo(DomainServiceType)
                || typeToRegister.IsAssignableTo(RepositoryType)
                || typeToRegister.IsAssignableToGenericType(DomainEventHanlderType))
               && !typeToRegister.IsGenericType
               && base.CanRegister(typeToRegister);
    }

    protected override ServiceLifetime? GetDefaultLifetime(Type type)
    {
        // TODO:Intereceptor is not working when respository services is scoped.
        return type.IsAssignableTo(RepositoryType) ? ServiceLifetime.Transient : ServiceLifetime.Transient;
    }

    protected override ICollection<Type> GetServiceTypes(Type implementationType)
    {
        // Domain event handler
        if (implementationType.IsAssignableToGenericType(DomainEventHanlderType))
        {
            return implementationType.GetInterfaces()
                                     .Where(v => v.IsAssignableToGenericType(DomainEventHanlderType))
                                     .ToArray();
        }

        // Domain service
        var types = base.GetServiceTypes(implementationType);
        if (implementationType.IsAssignableTo(DomainServiceType))
        {
            return types.Where(v => v.IsInterface && v != DomainServiceType).ToArray();
        }
        // Repository
        if (implementationType.IsAssignableTo(RepositoryType))
        {
            var ret = types.Where(v => v != RepositoryType).ToList();
            var readonlyRepoType = implementationType.GetInterfaces()
                                                     .Single(v => v.IsGenericType && v.GetGenericTypeDefinition() == typeof(IReadOnlyRepository<,>));
            ret.AddIfNotContains(readonlyRepoType);
            return ret;
        }

        throw new InvalidOperationException("Invalid domain type.");
    }
}