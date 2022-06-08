using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using Fabricdot.Core.Aspects;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.DependencyInjection;

public class DefaultDependencyRegistrar : IDependencyRegistrar
{
    public static readonly ISet<Type> IgnoredServiceTypes = new HashSet<Type>() { typeof(IInterceptor) };

    public virtual void Register(
        IServiceCollection services,
        Type implementationType)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(implementationType, nameof(implementationType));

        if (!CanRegister(implementationType))
            return;

        var lifetime = GetLifetime(implementationType);
        if (lifetime is null)
            return;

        var dependencyRegistry = GetDependencyRegistry(implementationType, lifetime.Value);
        dependencyRegistry.ToServiceDescriptors()
                          .ForEach(serviceDescriptor => services.Add(serviceDescriptor, dependencyRegistry.RegisterBehavior));
    }

    protected static ICollection<Type> GetDefaultServiceTypes(Type ImplementationType)
    {
        Guard.Against.Null(ImplementationType, nameof(ImplementationType));

        var ret = new List<Type>()
        {
            ImplementationType
        };
        // Add default contracts by name.
        foreach (var @interface in ImplementationType.GetTypeInfo().ImplementedInterfaces)
        {
            var interfaceName = @interface.IsGenericType ? @interface.Name.Split('`').First() : @interface.Name;
            if (ImplementationType.Name.EndsWith(interfaceName.TrimStart('I')))
                ret.Add(@interface);
        }

        return ret;
    }

    protected virtual bool CanRegister(Type typeToRegister)
    {
        return typeToRegister?.IsNonAbstractClass(false) == true
               && !typeToRegister.IsDefined(typeof(IgnoreDependencyAttribute), true);
    }

    protected virtual ICollection<Type> GetServiceTypes(Type implementationType)
    {
        var serviceTypes = implementationType.GetCustomAttributes(true)
                                             .OfType<ServiceContractAttribute>()
                                             .SelectMany(v => v.ContractTypes)
                                             .Distinct()
                                             .ToList();

        if (serviceTypes.IsNullOrEmpty())
            serviceTypes.AddRange(GetDefaultServiceTypes(implementationType));

        return serviceTypes;
    }

    protected virtual bool IgnoreServiceType(Type serviceType)
    {
        return IgnoredServiceTypes.Contains(serviceType) || serviceType.IsDefined(typeof(IgnoreDependencyAttribute));
    }

    protected virtual IDependencyRegistry GetDependencyRegistry(
        Type implementationType,
        ServiceLifetime serviceLifetime)
    {
        var depdencyAttr = implementationType.GetCustomAttribute<DependencyAttribute>(true);
        return new DependencyRegistry(
            implementationType,
            GetServiceTypes(implementationType).Where(v => !IgnoreServiceType(v)),
            serviceLifetime,
            depdencyAttr?.RegisterBehavior ?? default);
    }

    protected virtual ServiceLifetime? GetLifetime(Type type)
    {
        var depdencyAttr = type.GetCustomAttribute<DependencyAttribute>(true);
        if (depdencyAttr?.Lifetime is not null)
            return depdencyAttr.Lifetime;

        if (typeof(ITransientDependency).IsAssignableFrom(type))
            return ServiceLifetime.Transient;

        if (typeof(ISingletonDependency).IsAssignableFrom(type))
            return ServiceLifetime.Singleton;

        if (typeof(IScopedDependency).IsAssignableFrom(type))
            return ServiceLifetime.Scoped;

        return GetDefaultLifetime(type);
    }

    protected virtual ServiceLifetime? GetDefaultLifetime(Type type)
    {
        return null;
    }
}