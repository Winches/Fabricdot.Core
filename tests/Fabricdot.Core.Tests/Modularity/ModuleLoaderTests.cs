using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleLoaderTests : TestFor<ModuleLoader>
{
    [Requires(typeof(RecursiveModule))]
    private class RecursiveModule : ModuleBase
    {
    }

    [Fact]
    public void LoadModules_CorrectlyOrder()
    {
        var moduleCollection = Sut.LoadModules(typeof(FakeStartupModule));

        moduleCollection.Should()
                        .HaveCount(3).And
                        .Satisfy(v => v.Instance is FakeCoreModule, v => v.Instance is FakeInfrastructureModule, v => v.Instance is FakeStartupModule);
    }

    [Fact]
    public void LoadModules_CyclicDependency_ThrowException()
    {
        Invoking(() => Sut.LoadModules(typeof(RecursiveModule)))
                     .Should()
                     .Throw<ArgumentException>();
    }
}