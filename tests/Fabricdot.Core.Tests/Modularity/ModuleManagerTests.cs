using System;
using System.Threading.Tasks;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleManagerTests
{
    [Fact]
    public async Task StartAsync_WhenErrorOccurred_ThrowException()
    {
        var moduleManager = CreateModuleManager<FakeRuntimeErrorModule>();

        await FluentActions.Awaiting(() => moduleManager.StartAsync(null))
                           .Should()
                           .ThrowAsync<ModularityException>()
                           .WithInnerException(typeof(Exception))
                           .WithMessage(FakeRuntimeErrorModule.OnStartingErrorMessage);
    }

    [Fact]
    public async Task StopAsync_WhenErrorOccurred_ThrowException()
    {
        var moduleManager = CreateModuleManager<FakeRuntimeErrorModule>();

        await FluentActions.Awaiting(() => moduleManager.StopAsync(null))
                           .Should()
                           .ThrowAsync<ModularityException>()
                           .WithInnerException(typeof(Exception))
                           .WithMessage(FakeRuntimeErrorModule.OnStoppingErrorMessage);
    }

    [Fact]
    public void Modules_ReturnLoadedModules()
    {
        var moduleLoader = new ModuleLoader();
        var modules = moduleLoader.LoadModules(typeof(FakeRuntimeErrorModule));
        var moduleManager = new ModuleManager(NullLogger<ModuleManager>.Instance, modules);

        moduleManager.Modules.Should().BeEquivalentTo(modules);
    }

    private static ModuleManager CreateModuleManager<T>() where T : IModule
    {
        var moduleLoader = new ModuleLoader();
        var modules = moduleLoader.LoadModules(typeof(T));
        return new ModuleManager(NullLogger<ModuleManager>.Instance, modules);
    }
}