using System;
using System.Threading.Tasks;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Fabricdot.Core.Tests.Boot;

public class BootstrapperTests
{
    [Fact]
    public void CreateBootstrapper_ConfigureServiceFailed_ThrowException()
    {
        FluentActions.Invoking(() => CreateBootstrapper<FakeRuntimeErrorModule>())
                     .Should()
                     .Throw<ModularityException>()
                     .WithInnerException<Exception>()
                     .WithMessage(FakeRuntimeErrorModule.ConfigureServicesErrorMessage);
    }

    [Fact]
    public async Task StartAsync_InvokeMethod_TriggerModuleOnStartingAsync()
    {
        using var app = CreateBootstrapper<FakeStartupModule>();

        await app.StartAsync();
    }

    [Fact]
    public async Task StartAsync_InvokeTwice_ThrowException()
    {
        using var app = CreateBootstrapper<FakeStartupModule>();
        await app.StartAsync();

        await FluentActions.Awaiting(() => app.StartAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_InvokeTwice_ThrowException()
    {
        using var app = CreateBootstrapper<FakeStartupModule>();
        await app.StartAsync();
        await app.StopAsync();

        await FluentActions.Awaiting(() => app.StopAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_WithoutInvokeStartAsync_ThrowException()
    {
        using var app = CreateBootstrapper<FakeStartupModule>();

        await FluentActions.Awaiting(() => app.StopAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task StopAsync_InvokeMethod_TriggerModuleOnStoppingAsync()
    {
        using var app = CreateBootstrapper<FakeStartupModule>();

        await app.StartAsync();
        await app.StopAsync();
    }

    [Fact]
    public void SetServiceProvider_WhenValueExists_ThrowException()
    {
        var app = new Bootstrapper();
        var providerMock = new Mock<IServiceProvider>();
        app.SetServiceProvider(providerMock.Object);

        FluentActions.Invoking(() => app.SetServiceProvider(providerMock.Object))
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    private static IApplication CreateBootstrapper<T>() where T : IModule
    {
        var options = new BootstrapperBuilderOptions();
        return Bootstrapper.CreateBuilder(options)
                           .AddModules(typeof(T))
                           .Build(options.Services.BuildServiceProvider());
    }
}