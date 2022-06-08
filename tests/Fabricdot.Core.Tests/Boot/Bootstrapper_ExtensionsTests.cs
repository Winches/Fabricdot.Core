using System;
using System.Threading.Tasks;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Tests.Modules;
using Fabricdot.Core.Tests.Modules.Exports;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Core.Tests.Boot;

public class Bootstrapper_ExtensionsTests
{
    [Fact]
    public void AddBootstrapper_GivenModuleType_RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddBootstrapper<FakeStartupModule>();

        services.Should().ContainSingle(v => v.ServiceType == typeof(FakeService));
    }

    [Fact]
    public void AddBootstrapper_ConfigureOptions_Correctly()
    {
        var services = new ServiceCollection();
        services.AddBootstrapper<FakeStartupModule>(opts =>
       {
           opts.Services.Should().BeSameAs(services);
           opts.ConfigurationOptions.Should().NotBeNull();
       });
    }

    [Fact]
    public async Task BootstrapAsync_WhenAddApplication_StartCorrectly()
    {
        var services = new ServiceCollection();
        services.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = services.BuildServiceProvider();

        await serviceProvider.BootstrapAsync();
    }

    [Fact]
    public async Task BootstrapAsync_ConfigureApp_StartCorrectly()
    {
        var services = new ServiceCollection();
        services.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = services.BuildServiceProvider();

        await serviceProvider.BootstrapAsync(app =>
        {
            app.Should().NotBeNull();
            app.Services.GetService<IConfiguration>().Should().NotBeNull();
        });
    }

    [Fact]
    public async Task BootstrapAsync_WithoutAddBootsrapper_ThrowException()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        await FluentActions.Awaiting(() => serviceProvider.BootstrapAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetBuilderProperties_WhenAddApplication_ReturnCorrectly()
    {
        var services = new ServiceCollection();
        var builder = services.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = services.BuildServiceProvider();
        await serviceProvider.BootstrapAsync();

        serviceProvider.GetBuilderProperties()
                       .Should()
                       .BeSameAs(builder.Properties);
    }
}