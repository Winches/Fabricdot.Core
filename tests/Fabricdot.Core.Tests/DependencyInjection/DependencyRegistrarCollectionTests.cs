using Fabricdot.Core.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity
{
    public class DependencyRegistrarCollectionTests
    {
        [Fact]
        public void AddDependencyRegistrar_WhenRegistrarEixsts_IgnoreDuplicatedRegistrar()
        {
            var services = new ServiceCollection();
            services.GetSingletonInstance<IDependencyRegistrarCollection>().Should().BeNull();
            services.AddDependencyRegistrar<DefaultDependencyRegistrar>();
            services.AddDependencyRegistrar<DefaultDependencyRegistrar>();
            services.GetSingletonInstance<IDependencyRegistrarCollection>()
                    .Should()
                    .NotBeNull().And
                    .OnlyHaveUniqueItems();
        }
    }
}