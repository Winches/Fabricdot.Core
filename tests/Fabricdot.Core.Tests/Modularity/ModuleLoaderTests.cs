using System;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleLoaderTests
{
    [Requires(typeof(RecursiveModule))]
    private class RecursiveModule : ModuleBase
    {
    }

    [Fact]
    public void LoadModules_CorrectlyOrder()
    {
        var moduleLoader = new ModuleLoader();
        var moduleCollection = moduleLoader.LoadModules(typeof(FakeStartupModule));

        Assert.Equal(3, moduleCollection.Count);
        Assert.True(moduleCollection[0].Instance is FakeCoreModule);
        Assert.True(moduleCollection[2].Instance is FakeStartupModule);
    }

    [Fact]
    public void LoadModules_CyclicDependency_ThrowException()
    {
        var moduleLoader = new ModuleLoader();
        void testCode() => moduleLoader.LoadModules(typeof(RecursiveModule));
        Assert.Throws<ArgumentException>(testCode);
    }
}