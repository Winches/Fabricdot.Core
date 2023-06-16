using FluentAssertions.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace FluentAssertions;

public static class ServiceCollectionAssertionExtensions
{
    public static AndWhichConstraint<GenericCollectionAssertions<ServiceDescriptor>, ServiceDescriptor> Contain<TService>(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        ServiceLifetime? lifetime = null)
    {
        return assertions.Contain(typeof(TService), lifetime);
    }

    public static AndWhichConstraint<GenericCollectionAssertions<ServiceDescriptor>, ServiceDescriptor> Contain(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        Type serviceType,
        ServiceLifetime? lifetime = null)
    {
        return assertions.Contain(v => v.ServiceType == serviceType && (!lifetime.HasValue || v.Lifetime == lifetime));
    }

    public static AndWhichConstraint<GenericCollectionAssertions<ServiceDescriptor>, ServiceDescriptor> ContainSingle(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        Type serviceType,
        ServiceLifetime? lifetime = null)
    {
        return assertions.ContainSingle(v => v.ServiceType == serviceType && (!lifetime.HasValue || v.Lifetime == lifetime));
    }

    public static AndWhichConstraint<GenericCollectionAssertions<ServiceDescriptor>, ServiceDescriptor> ContainSingle<TService>(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        ServiceLifetime? lifetime = null)
    {
        return assertions.ContainSingle(typeof(TService), lifetime);
    }

    public static AndConstraint<GenericCollectionAssertions<ServiceDescriptor>> NotContain(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        Type serviceType,
        ServiceLifetime? lifetime = null)
    {
        return assertions.NotContain(v => v.ServiceType == serviceType && (!lifetime.HasValue || v.Lifetime == lifetime));
    }

    public static AndConstraint<GenericCollectionAssertions<ServiceDescriptor>> NotContain<TService>(
        this GenericCollectionAssertions<ServiceDescriptor> assertions,
        ServiceLifetime? lifetime = null)
    {
        return assertions.NotContain(typeof(TService), lifetime);
    }
}
