using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Modularity;

public class DependencyRegistrarCollectionTests : TestBase
{
    [Fact]
    public void AddDependencyRegistrar_WhenRegistrarEixsts_IgnoreDuplicatedRegistrar()
    {
        var services = new ServiceCollection();
        services.AddDependencyRegistrar<DefaultDependencyRegistrar>();
        services.AddDependencyRegistrar<DefaultDependencyRegistrar>();

        services.GetSingletonInstance<IDependencyRegistrarCollection>()
                .Should()
                .NotBeNull().And
                .OnlyHaveUniqueItems();
    }
}