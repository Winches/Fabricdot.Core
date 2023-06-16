using Fabricdot.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Fabricdot.Core.Tests.Configuration;

public class ConfigurationFactoryTests : TestFor<ConfigurationBuilderOptions>
{
    [InlineData("Development")]
    [InlineData("Production")]
    [Theory]
    public void Build_GivenEnvironment_LoadSpecificSettings(string env)
    {
        Sut.EnvironmentName = env;
        var configuration = ConfigurationFactory.Build(Sut);

        configuration.Should().NotBeNull();
        configuration.GetValue<string>("Env").Should().Be(env);
    }

    [AutoData]
    [Theory]
    public void Build_GivenCommandLineArgs_LoadArgs(
        string key,
        int value)
    {
        Sut.CommandLineArgs = new[] { $"{key}={value}" };
        var configuration = ConfigurationFactory.Build(Sut);

        configuration.Should().NotBeNull();
        configuration.GetValue<int>(key).Should().Be(value);
    }

    [AutoData]
    [Theory]
    public void Build_GivenAction_InvokeAction(
        string key,
        int value)
    {
        var configuration = ConfigurationFactory.Build(Sut, builder => builder.AddCommandLine(new[] { $"{key}={value}" }));

        configuration.Should().NotBeNull();
        configuration.GetValue<int>(key).Should().Be(value);
    }

    protected override ConfigurationBuilderOptions CreateSut()
    {
        return Fixture.Build<ConfigurationBuilderOptions>().OmitAutoProperties().Create();
    }
}
