using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleCollectionTests : TestBase
{
    [Fact]
    public void Build_CannotSolveDependency_ThrowException()
    {
        var modules = new ModuleCollection
        {
            new ModuleMetadata(typeof(FakeStartupModule),
            new FakeStartupModule())
        };

        Invoking(() => modules.Build())
                     .Should()
                     .Throw<ModularityException>();
    }
}
