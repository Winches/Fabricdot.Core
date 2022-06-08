using System;
using Fabricdot.Core.Tests.Modules.Exports.Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Core.Tests.Microsoft.Extensions.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ContainsService_GivenType_ReturnCorrectly()
    {
        var services = new ServiceCollection();
        services.AddSingleton<object>(1);

        services.ContainsService<object>().Should().BeTrue();
        services.ContainsService<FakeCoreService>().Should().BeFalse();
    }

    [Fact]
    public void GetSingletonInstance_WhenServiceExists_ReturnInstance()
    {
        const int expected = 1;
        var services = new ServiceCollection();
        services.AddSingleton<object>(expected);

        services.GetSingletonInstance<object>().Should().Be(expected);
    }

    [Fact]
    public void GetSingletonInstance_WhenServiceNotExists_ReturnNull()
    {
        var services = new ServiceCollection();

        services.GetSingletonInstance<object>().Should().BeNull();
    }

    [Fact]
    public void GetRequiredSingletonInstance_WhenServiceNotExists_ThrowException()
    {
        var services = new ServiceCollection();

        FluentActions.Invoking(services.GetRequiredSingletonInstance<object>)
                     .Should()
                     .Throw<InvalidOperationException>();
    }
}