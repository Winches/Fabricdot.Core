using System;
using System.Linq;
using System.Reflection;
using Fabricdot.Common.Core.Reflections;
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
            Reflection.FindTypes(repositoryType, assemblies)
                .ForEach(v =>
                {
                    var contract = v.GetInterfaces()
                        .FirstOrDefault(i => i.IsInterface && !i.IsGenericType && i.IsAssignableToGenericType(repositoryType));
                    if (contract == null)
                        throw new InvalidOperationException($"{v.Name} should implement owned repository interface.");
                    services.TryAddScoped(contract, v);
                });
            return services;
        }

        public static IServiceCollection AddDomainServices(
            [NotNull] this IServiceCollection services,
            params Assembly[] assemblies)
        {
            var serviceType = typeof(IDomainService);
            Reflection.FindTypes(serviceType, assemblies)
                .ForEach(v =>
                {
                    var contract = v.GetInterfaces()
                        .FirstOrDefault(i => i.IsInterface && serviceType.IsAssignableFrom(i));
                    if (contract == null)
                        throw new InvalidOperationException($"{v.Name} should implement owned service interface.");
                    services.TryAddScoped(contract, v);
                });
            return services;
        }
    }
}