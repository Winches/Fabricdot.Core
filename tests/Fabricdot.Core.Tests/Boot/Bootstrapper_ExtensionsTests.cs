using Fabricdot.Core.Boot;
using Fabricdot.Core.Tests.Modules;
using Fabricdot.Core.Tests.Modules.Exports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Boot;

public class Bootstrapper_ExtensionsTests : TestFor<ServiceCollection>
{
    [Fact]
    public void AddBootstrapper_GivenModuleType_RegisterServices()
    {
        Sut.AddBootstrapper<FakeStartupModule>();

        Sut.Should().ContainSingle<FakeService>();
    }

    [Fact]
    public void AddBootstrapper_ConfigureOptions_Correctly()
    {
        Sut.AddBootstrapper<FakeStartupModule>(opts =>
       {
           opts.Services.Should().BeSameAs(Sut);
           opts.ConfigurationOptions.Should().NotBeNull();
       });
    }

    [Fact]
    public async Task BootstrapAsync_WhenAddApplication_StartCorrectly()
    {
        Sut.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = Sut.BuildServiceProvider();

        await serviceProvider.BootstrapAsync();
    }

    [Fact]
    public async Task BootstrapAsync_ConfigureApp_StartCorrectly()
    {
        Sut.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = Sut.BuildServiceProvider();

        await serviceProvider.BootstrapAsync(app =>
        {
            app.Should().NotBeNull();
            app.Services.GetService<IConfiguration>().Should().NotBeNull();
        });
    }

    [Fact]
    public async Task BootstrapAsync_WithoutAddBootsrapper_ThrowException()
    {
        var serviceProvider = Sut.BuildServiceProvider();

        await Awaiting(() => serviceProvider.BootstrapAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetBuilderProperties_WhenAddApplication_ReturnCorrectly()
    {
        var builder = Sut.AddBootstrapper<FakeStartupModule>();
        var serviceProvider = Sut.BuildServiceProvider();
        await serviceProvider.BootstrapAsync();

        serviceProvider.GetBuilderProperties()
                       .Should()
                       .BeSameAs(builder.Properties);
    }
}