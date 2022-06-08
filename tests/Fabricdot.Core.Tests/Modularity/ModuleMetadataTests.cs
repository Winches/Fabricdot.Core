using System;
using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleMetadataTests
{
    [Fact]
    public void Constructor_WhenInstanceTypeIsNotMath_ThrowException()
    {
        static void action() => _ = new ModuleMetadata(typeof(FakeStartupModule), new FakeInfrastructureModule());

        FluentActions.Invoking(action)
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_GivenDependencies_Correctly()
    {
        var dependency = new ModuleMetadata(typeof(FakeInfrastructureModule), new FakeInfrastructureModule());
        var module = new ModuleMetadata(
            nameof(FakeStartupModule),
            typeof(FakeStartupModule),
            new FakeStartupModule(),
            dependency,
            dependency);

        module.Dependencies.Should().ContainSingle(v => v.Equals(dependency));
    }

    [Fact]
    public void Equals_GivenInput_ReturnCorrectly()
    {
        var module = new ModuleMetadata(
            nameof(FakeStartupModule),
            typeof(FakeStartupModule),
            new FakeStartupModule());
        var dependency = new ModuleMetadata(typeof(FakeInfrastructureModule), new FakeInfrastructureModule());

        module.Equals(module).Should().BeTrue();
        module.Equals(null).Should().BeFalse();
        module.Equals(dependency).Should().BeFalse();
    }
}