using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleManagerTests : TestBase
{
    [Fact]
    public async Task StartAsync_WhenErrorOccurred_ThrowException()
    {
        var moduleManager = CreateModuleManager<FakeRuntimeErrorModule>();

        await Awaiting(() => moduleManager.StartAsync(null!))
                           .Should()
                           .ThrowAsync<ModularityException>()
                           .WithInnerException(typeof(Exception))
                           .WithMessage(FakeRuntimeErrorModule.OnStartingErrorMessage);
    }

    [Fact]
    public async Task StopAsync_WhenErrorOccurred_ThrowException()
    {
        var moduleManager = CreateModuleManager<FakeRuntimeErrorModule>();

        await Awaiting(() => moduleManager.StopAsync(null!))
                           .Should()
                           .ThrowAsync<ModularityException>()
                           .WithInnerException(typeof(Exception))
                           .WithMessage(FakeRuntimeErrorModule.OnStoppingErrorMessage);
    }

    [Fact]
    public void Modules_ReturnLoadedModules()
    {
        var modules = Create<ModuleCollection>();
        Fixture.AddManyTo(modules, 5);
        var moduleManager = new ModuleManager(Create<ILogger<ModuleManager>>(), modules);

        moduleManager.Modules.Should().BeEquivalentTo(modules);
    }

    private ModuleManager CreateModuleManager<T>() where T : IModule
    {
        var moduleLoader = new ModuleLoader();
        var modules = moduleLoader.LoadModules(typeof(T));
        return new ModuleManager(Create<ILogger<ModuleManager>>(), modules);
    }
}
