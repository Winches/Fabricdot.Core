using Fabricdot.Infrastructure.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Commands;

public class CommandBusTests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly ICommandBus _commandBus;

    public CommandBusTests()
    {
        _commandBus = ServiceProvider.GetRequiredService<ICommandBus>();
    }

    [Fact]
    public async Task PublishAsync_GivenNull_Throw()
    {
        await _commandBus.Awaiting(v => v.PublishAsync<object>(null))
                         .Should()
                         .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task PublishAsync_WithoutHandler_Throw()
    {
        await _commandBus.Awaiting(v => v.PublishAsync(new NoHandlerCommand()))
                         .Should()
                         .ThrowAsync<Exception>();
    }

    [Fact]
    public async Task PublishAsync_Should_InvokeHandler()
    {
        var command = new SimpleCommand();
        _ = await _commandBus.PublishAsync(command);

        command.Invoked.Should().BeTrue();
    }

    [AutoData]
    [Theory]
    public async Task PublishAsync_Should_ReturnValue(int left, int right)
    {
        var expected = left + right;
        var command = new AddValueCommand(left, right);
        var ret = await _commandBus.PublishAsync(command);

        ret.Should().Be(expected);
    }
}