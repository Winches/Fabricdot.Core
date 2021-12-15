using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Fabricdot.Core.Reflection;
using Fabricdot.Domain.Core.Events;
using Fabricdot.Domain.Core.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Infrastructure.Core.Domain
{
    public static class DomainDependencyRegistrar
    {
        public static IServiceCollection AddRepositories(
            [NotNull] this IServiceCollection services,
            params Assembly[] assemblies)
        {
            var repositoryType = typeof(IRepository<,>);
            ReflectionHelper.FindTypes(repositoryType, assemblies)
                .ForEach(implementationType =>
                {
                    var serviceType = implementationType.GetInterfaces()
                        .FirstOrDefault(i => i.IsInterface && !i.IsGenericType && i.IsAssignableToGenericType(repositoryType));
                    if (serviceType == null)
                    {
                        Debug.Print($"{implementationType.Name} do not implement non-generic repository interface.");
                        return;
                    }

                    var readonlyRepositoryType = serviceType.GetInterfaces()
                                                            .Single(v => v.IsGenericType && v.GetGenericTypeDefinition() == typeof(IReadOnlyRepository<,>));
                    services.TryAddScoped(serviceType, implementationType);
                    services.TryAddScoped(readonlyRepositoryType, implementationType);
                });
            return services;
        }

        public static IServiceCollection AddDomainServices(
            [NotNull] this IServiceCollection services,
            params Assembly[] assemblies)
        {
            var domainServiceType = typeof(IDomainService);
            ReflectionHelper.FindTypes(domainServiceType, assemblies)
                .ForEach(implementationType =>
                {
                    var serviceType = implementationType.GetInterfaces()
                        .FirstOrDefault(i => i.IsInterface && domainServiceType.IsAssignableFrom(i));
                    if (serviceType == null)
                    {
                        Debug.Print($"{implementationType.Name} do not implement non-generic service interface.");
                        return;
                    }
                    services.TryAddScoped(serviceType, implementationType);
                });
            return services;
        }

        public static IServiceCollection AddDomainEventHandlers(
            [NotNull] this IServiceCollection services,
            params Assembly[] assemblies)
        {
            var eventHandlerType = typeof(IDomainEventHandler<>);
            ReflectionHelper.FindTypes(eventHandlerType, assemblies)
                .ForEach(implementationType =>
                {
                    var serviceType = implementationType.GetInterfaces()
                        .First(i => i.IsInterface && i.IsGenericType && i.GetGenericTypeDefinition() == eventHandlerType);
                    services.AddTransient(serviceType, implementationType);
                });
            return services;
        }
    }
}