using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Boot;

public class BootstrapperTests : TestBase
{
    [Fact]
    public void CreateBootstrapper_ConfigureServiceFailed_ThrowException()
    {
        Invoking(() => CreateApp<FakeRuntimeErrorModule>())
                     .Should()
                     .Throw<ModularityException>()
                     .WithInnerException<Exception>()
                     .WithMessage(FakeRuntimeErrorModule.ConfigureServicesErrorMessage);
    }

    [Fact]
    public async Task StartAsync_InvokeMethod_TriggerModuleOnStartingAsync()
    {
        using var app = CreateApp<FakeStartupModule>();

        await app.StartAsync();
    }

    [Fact]
    public async Task StartAsync_InvokeTwice_ThrowException()
    {
        using var app = CreateApp<FakeStartupModule>();
        await app.StartAsync();

        await Awaiting(() => app.StartAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_InvokeTwice_ThrowException()
    {
        using var app = CreateApp<FakeStartupModule>();
        await app.StartAsync();
        await app.StopAsync();

        await Awaiting(() => app.StopAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_WithoutInvokeStartAsync_ThrowException()
    {
        using var app = CreateApp<FakeStartupModule>();

        await Awaiting(() => app.StopAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_InvokeMethod_TriggerModuleOnStoppingAsync()
    {
        using var app = CreateApp<FakeStartupModule>();

        await app.StartAsync();
        await app.StopAsync();
    }

    [Fact]
    public void SetServiceProvider_WhenValueExists_ThrowException()
    {
        var app = new Bootstrapper();
        var provider = Create<IServiceProvider>();
        app.SetServiceProvider(Create<IServiceProvider>());

        Invoking(() => app.SetServiceProvider(provider))
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    private static IApplication CreateApp<T>() where T : IModule
    {
        var options = new BootstrapperBuilderOptions();
        return Bootstrapper.CreateBuilder(options)
                           .AddModules(typeof(T))
                           .Build(options.Services.BuildServiceProvider());
    }
}