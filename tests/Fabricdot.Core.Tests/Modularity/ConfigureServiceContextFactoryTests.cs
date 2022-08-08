using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fabricdot.Core.Tests.Modularity;

public class ConfigureServiceContextFactoryTests : TestBase
{
    public ConfigureServiceContextFactoryTests()
    {
        InjectMock<IBootstrapperBuilder>().SetupGet(v => v.Services)
                                          .Returns(new ServiceCollection());
    }

    [Fact]
    public void Constructor_GivenNull_ThrowException()
    {
        var sut = typeof(ConfigureServiceContextFactory).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Create_WhenHostBuilderContexExists_UseConfiguration()
    {
        var bootstrapperBuilder = Create<IBootstrapperBuilder>();
        var hostContext = Create<HostBuilderContext>();
        bootstrapperBuilder.Services.AddSingleton(hostContext);
        var expected = new { hostContext.Configuration };

        var configureServiceContextFactory = new ConfigureServiceContextFactory(bootstrapperBuilder);
        var configureServiceContext = configureServiceContextFactory.Create();

        configureServiceContext.Should()
                               .NotBeNull().And
                               .BeEquivalentTo(expected);
    }

    [Fact]
    public void Create_WhenConfigurationExists_UseConfiguration()
    {
        var bootstrapperBuilder = Create<IBootstrapperBuilder>();
        var services = bootstrapperBuilder.Services;
        var configuration = Create<IConfiguration>();
        services.AddSingleton(configuration);
        var expected = new { Configuration = configuration };

        var configureServiceContextFactory = new ConfigureServiceContextFactory(bootstrapperBuilder);
        var configureServiceContext = configureServiceContextFactory.Create();

        services.GetSingletonInstance<IConfiguration>()
                .Should()
                .NotBeNull();
        configureServiceContext.Should()
                               .NotBeNull().And
                               .BeEquivalentTo(expected);
    }

    [Fact]
    public void Create_WhenConfigurationNotExists_CreateConfiguration()
    {
        var configureServiceContextFactory = Create<ConfigureServiceContextFactory>();
        var configureServiceContext = configureServiceContextFactory.Create();
        var services = configureServiceContext.Services;

        services.GetSingletonInstance<IConfiguration>().Should().BeNull();
        services.GetSingletonInstance<HostBuilderContext>().Should().BeNull();
        configureServiceContext.Should().NotBeNull();
        configureServiceContext.Configuration.Should().NotBeNull();
    }
}