using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Modularity;

public class DefaultDependencyRegistratrTests : TestFor<ServiceCollection>
{
    [Dependency(RegisterBehavior = RegistrationBehavior.TryAdd)]
    internal class FooService : IBarService, IFooService<string>, IScopedDependency
    {
    }

    [IgnoreDependency]
    internal class IgnoredFooService : FooService
    {
    }

    [Dependency]
    internal class FooSingletonService : ISingletonDependency
    {
    }

    [Dependency(ServiceLifetime.Transient)]
    internal class FooTransientService : IBarService, ISingletonDependency
    {
    }

    [Dependency]
    internal class FooWithoutLifetimeService
    {
    }

    [ServiceContract(typeof(FooService))]
    [Dependency(RegisterBehavior = RegistrationBehavior.Replace)]
    internal class FooDerivedService : FooService
    {
    }

    public DefaultDependencyRegistratrTests()
    {
        Sut.AddDependencyRegistrar<DefaultDependencyRegistrar>();
    }

    internal interface IFooService
    {
    }

    [IgnoreDependency]
    internal interface IBarService : IFooService
    {
    }

    internal interface IFooService<T> : IFooService
    {
    }

    [Fact]
    public void AddType_GivenInvalidType_IngoreType()
    {
        var typeWithoutLifetime = typeof(FooWithoutLifetimeService);
        Sut.AddType(typeWithoutLifetime);
        var nestedType = typeof(IgnoredFooService);
        Sut.AddType(nestedType);

        Sut.Should().NotContain(typeWithoutLifetime);
        Sut.Should().NotContain(nestedType);
    }

    [Fact]
    public void AddType_GivenClass_RegisterCorrectLifetime()
    {
        var transientType = typeof(FooTransientService);
        var scopedType = typeof(FooService);
        var singletonType = typeof(FooSingletonService);
        Sut.AddType(transientType);
        Sut.AddType(scopedType);
        Sut.AddType(singletonType);

        Sut.Should().ContainSingle(transientType, ServiceLifetime.Transient);
        Sut.Should().ContainSingle(scopedType, ServiceLifetime.Scoped);
        Sut.Should().ContainSingle(singletonType, ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddType_GivenClass_RegisterDefaultServices()
    {
        var implementationType = typeof(FooService);
        Sut.AddType(implementationType);

        Sut.Should().Contain(implementationType);
        Sut.Should().Contain<IFooService>();
        Sut.Should().Contain<IFooService<string>>();
    }

    [Fact]
    public void AddType_GivenClass_IgnoreSpecificServiceType()
    {
        var serviceType1 = typeof(IFooService<string>);
        var implementationType = typeof(FooService);
        DefaultDependencyRegistrar.IgnoredServiceTypes.Add(serviceType1);
        Sut.AddType(implementationType);

        Sut.Should().ContainSingle(implementationType);
        Sut.Should().NotContain(serviceType1);
        Sut.Should().Contain<IFooService>();
    }

    [Fact]
    public void AddType_GivenClass_TryAddService()
    {
        var implementationType = typeof(FooService);
        Sut.AddType(implementationType);
        Sut.AddType(implementationType);

        Sut.Should().ContainSingle(implementationType);
    }

    [Fact]
    public void AddType_GivenClass_ReplaceService()
    {
        var originType = typeof(FooService);
        var replacedType = typeof(FooDerivedService);
        Sut.AddType(originType);
        Sut.AddType(replacedType);

        Sut.Should().ContainSingle(v => v.ServiceType == originType && v.ImplementationType == replacedType);
    }
}