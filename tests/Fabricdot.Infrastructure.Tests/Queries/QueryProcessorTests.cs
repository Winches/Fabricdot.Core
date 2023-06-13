using Fabricdot.Infrastructure.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Queries;

public class QueryProcessorTests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IQueryProcessor _queryProcessor;

    public QueryProcessorTests()
    {
        _queryProcessor = ServiceProvider.GetRequiredService<IQueryProcessor>();
    }

    [Fact]
    public async Task PublishAsync_GivenNull_Throw()
    {
        await _queryProcessor.Awaiting(v => v.ProcessAsync<object>(null!))
                         .Should()
                         .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task PublishAsync_WithoutHandler_Throw()
    {
        await _queryProcessor.Awaiting(v => v.ProcessAsync(new NoHandlerQuery()))
                         .Should()
                         .ThrowAsync<Exception>();
    }

    [AutoData]
    [Theory]
    public async Task PublishAsync_Should_InvokeHandler(int expected)
    {
        var command = new SimpleQuery(expected);
        var actual = await _queryProcessor.ProcessAsync(command);

        actual.Should().Be(expected);
    }
}
