using Fabricdot.Core.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Fabricdot.Core.Tests.Configuration;

public class ConfigurationFactoryTests
{
    [InlineData("Development")]
    [InlineData("Production")]
    [Theory]
    public void Build_GivenEnvironment_LoadSpecificSettings(string env)
    {
        var options = new ConfigurationBuilderOptions()
        {
            EnvironmentName = env
        };
        var configuration = ConfigurationFactory.Build(options);

        configuration.Should().NotBeNull();
        configuration.GetValue<string>("Env").Should().Be(env);
    }

    [Fact]
    public void Build_GivenCommandLineArgs_LoadArgs()
    {
        const string key = "Name1";
        const int value = 1;
        var options = new ConfigurationBuilderOptions()
        {
            CommandLineArgs = new[] { $"{key}={value}" }
        };
        var configuration = ConfigurationFactory.Build(options);

        configuration.Should().NotBeNull();
        configuration.GetValue<int>(key).Should().Be(value);
    }

    [Fact]
    public void Build_GivenAction_InvokeAction()
    {
        const string key = "Name1";
        const int value = 1;
        var configuration = ConfigurationFactory.Build(null, builder => builder.AddCommandLine(new[] { $"{key}={value}" }));

        configuration.Should().NotBeNull();
        configuration.GetValue<int>(key).Should().Be(value);
    }
}