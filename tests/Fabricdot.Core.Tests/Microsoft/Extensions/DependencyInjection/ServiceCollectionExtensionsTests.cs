using Fabricdot.Core.Tests.Modules.Exports.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Microsoft.Extensions.DependencyInjection;

public class ServiceCollectionExtensionsTests : TestFor<ServiceCollection>
{
    [AutoData]
    [Theory]
    public void ContainsService_GivenType_ReturnCorrectly(object service)
    {
        Sut.AddSingleton(service);

        Sut.ContainsService<object>().Should().BeTrue();
        Sut.ContainsService<FakeCoreService>().Should().BeFalse();
    }

    [AutoData]
    [Theory]
    public void GetSingletonInstance_WhenServiceExists_ReturnInstance(object expected)
    {
        Sut.AddSingleton(expected);

        Sut.GetSingletonInstance<object>().Should().Be(expected);
    }

    [Fact]
    public void GetSingletonInstance_WhenServiceNotExists_ReturnNull()
    {
        Sut.GetSingletonInstance<object>().Should().BeNull();
    }

    [Fact]
    public void GetRequiredSingletonInstance_WhenServiceNotExists_ThrowException()
    {
        Invoking(Sut.GetRequiredSingletonInstance<object>)
                     .Should()
                     .Throw<InvalidOperationException>();
    }
}
