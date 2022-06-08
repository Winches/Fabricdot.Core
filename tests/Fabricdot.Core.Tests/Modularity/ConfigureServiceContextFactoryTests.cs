using System;
using System.Collections.Generic;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity;

public class ConfigureServiceContextFactoryTests
{
    [Fact]
    public void Constructor_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => new ConfigureServiceContextFactory(null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_WhenHostBuilderContexExists_UseConfiguration()
    {
        var bootstrapperBuilderMock = CreateBootstrapperBuilder();
        var services = bootstrapperBuilderMock.Object.Services;
        var hostContext = new HostBuilderContext(new Dictionary<object, object>())
        {
            Configuration = new Mock<IConfiguration>().Object
        };
        services.AddSingleton(hostContext);

        var configureServiceContextFactory = new ConfigureServiceContextFactory(bootstrapperBuilderMock.Object);
        var configureServiceContext = configureServiceContextFactory.Create();

        configureServiceContext.Should()
                               .NotBeNull().And
                               .BeEquivalentTo(new { hostContext.Configuration });
    }

    [Fact]
    public void Create_WhenConfigurationExists_UseConfiguration()
    {
        var bootstrapperBuilderMock = CreateBootstrapperBuilder();
        var services = bootstrapperBuilderMock.Object.Services;
        var mockConfiguration = new Mock<IConfiguration>();
        services.AddSingleton(mockConfiguration.Object);

        var configureServiceContextFactory = new ConfigureServiceContextFactory(bootstrapperBuilderMock.Object);
        var configureServiceContext = configureServiceContextFactory.Create();

        services.GetSingletonInstance<IConfiguration>()
                .Should()
                .NotBeNull();
        configureServiceContext.Should()
                               .NotBeNull().And
                               .BeEquivalentTo(new { Configuration = mockConfiguration.Object });
    }

    [Fact]
    public void Create_WhenConfigurationNotExists_CreateConfiguration()
    {
        var bootstrapperBuilderMock = CreateBootstrapperBuilder();
        var services = bootstrapperBuilderMock.Object.Services;

        var configureServiceContextFactory = new ConfigureServiceContextFactory(bootstrapperBuilderMock.Object);
        var configureServiceContext = configureServiceContextFactory.Create();

        services.GetSingletonInstance<IConfiguration>().Should().BeNull();
        services.GetSingletonInstance<HostBuilderContext>().Should().BeNull();
        configureServiceContext.Should().NotBeNull();
        configureServiceContext.Configuration.Should().NotBeNull();
    }

    private static Mock<IBootstrapperBuilder> CreateBootstrapperBuilder()
    {
        var services = new ServiceCollection();
        var bootstrapperBuilderMock = new Mock<IBootstrapperBuilder>();
        bootstrapperBuilderMock.SetupGet(v => v.Services).Returns(services);

        return bootstrapperBuilderMock;
    }
}