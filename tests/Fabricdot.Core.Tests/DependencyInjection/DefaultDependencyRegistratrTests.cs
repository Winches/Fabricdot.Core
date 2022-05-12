using Fabricdot.Core.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity
{
    public class DefaultDependencyRegistratrTests
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
            var services = CreateServices();
            var typeWithoutLifetime = typeof(FooWithoutLifetimeService);
            services.AddType(typeWithoutLifetime);
            var nestedType = typeof(IgnoredFooService);
            services.AddType(nestedType);

            services.Should().NotContain(v => v.ServiceType == typeWithoutLifetime);
            services.Should().NotContain(v => v.ServiceType == nestedType);
        }

        [Fact]
        public void AddType_GivenClass_RegisterCorrectLifetime()
        {
            var services = CreateServices();
            var transientType = typeof(FooTransientService);
            var scopedType = typeof(FooService);
            var singletonType = typeof(FooSingletonService);
            services.AddType(transientType);
            services.AddType(scopedType);
            services.AddType(singletonType);

            services.Should().ContainSingle(v => v.ServiceType == transientType && v.Lifetime == ServiceLifetime.Transient);
            services.Should().ContainSingle(v => v.ServiceType == scopedType && v.Lifetime == ServiceLifetime.Scoped);
            services.Should().ContainSingle(v => v.ServiceType == singletonType && v.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddType_GivenClass_RegisterDefaultServices()
        {
            var services = CreateServices();
            var implementationType = typeof(FooService);
            services.AddType(implementationType);

            services.Should().Contain(v => v.ServiceType == implementationType);
            services.Should().Contain(v => v.ServiceType == typeof(IFooService));
            services.Should().Contain(v => v.ServiceType == typeof(IFooService<string>));
        }

        [Fact]
        public void AddType_GivenClass_IgnoreSpecificServiceType()
        {
            var services = CreateServices();
            var serviceType1 = typeof(IFooService);
            var serviceType2 = typeof(IFooService);
            var implementationType = typeof(FooTransientService);
            DefaultDependencyRegistrar.IgnoredServiceTypes.Add(serviceType1);
            services.AddType(implementationType);

            services.Should().ContainSingle(v => v.ServiceType == implementationType);
            services.Should().NotContain(v => v.ServiceType == serviceType1);
            services.Should().NotContain(v => v.ServiceType == serviceType2);
        }

        [Fact]
        public void AddType_GivenClass_TryAddService()
        {
            var services = CreateServices();
            var implementationType = typeof(FooService);
            services.AddType(implementationType);
            services.AddType(implementationType);

            services.Should().ContainSingle(v => v.ServiceType == implementationType);
        }

        [Fact]
        public void AddType_GivenClass_ReplaceService()
        {
            var services = CreateServices();
            var originType = typeof(FooService);
            var replacedType = typeof(FooDerivedService);
            services.AddType(originType);
            services.AddType(replacedType);

            services.Should().ContainSingle(v => v.ServiceType == originType && v.ImplementationType == replacedType);
        }

        private static ServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddDependencyRegistrar<DefaultDependencyRegistrar>();
            return services;
        }
    }
}