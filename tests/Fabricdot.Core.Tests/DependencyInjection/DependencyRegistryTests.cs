using System.Collections.Generic;
using Fabricdot.Core.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity;

public class DependencyRegistryTests
{
    internal class CorgiDog : IDog, IAnimal, ICorgiDog<string>, ITransientDependency
    {
    }

    internal interface IAnimal
    {
    }

    internal interface IDog
    {
    }

    internal interface ICorgiDog<T> : IDog
    {
    }

    [Fact]
    public void Constructor_GivenServiceTyps_KeepServiceTypes()
    {
        var implementationType = typeof(CorgiDog);
        var registry = new DependencyRegistry(
            implementationType,
            new[] { typeof(IDog) },
            ServiceLifetime.Transient,
            RegistrationBehavior.Default);
        var serviceTypes = registry.ServiceTypes;

        serviceTypes.Should().HaveCount(1);
        serviceTypes.Should().Contain(typeof(IDog));
    }

    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Singleton)]
    [Theory]
    public void ToServiceDescriptors_MultipleScopedSingletonService_RedirectToSelf(ServiceLifetime lifetime)
    {
        var implementationType = typeof(CorgiDog);
        var serviceTypes = new[] { typeof(IDog), typeof(ICorgiDog<string>) };
        var registry = new DependencyRegistry(
            implementationType,
            serviceTypes,
            lifetime,
            RegistrationBehavior.Default);
        var serviceDescriptors = registry.ToServiceDescriptors();
        var services = new ServiceCollection
        {
            serviceDescriptors
        };
        var serviceProvider = services.BuildServiceProvider();

        serviceDescriptors.Should().HaveCount(3);
        serviceDescriptors.Should().OnlyContain(v => v.Lifetime == lifetime);
        serviceTypes.ForEach(serviceType => serviceProvider.GetService(serviceType)
                                                           .Should()
                                                           .BeOfType(implementationType));
    }
}