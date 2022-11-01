using Fabricdot.Infrastructure.Queries;

namespace Fabricdot.Infrastructure.Tests.Queries;

internal class SimpleQuery : Query<int>
{
    public int Value { get; }

    public SimpleQuery(int value)
    {
        Value = value;
    }
}

internal class SimpleQueryHandler : QueryHandler<SimpleQuery, int>
{
    public override Task<int> ExecuteAsync(
        SimpleQuery query,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(query.Value);
    }
}