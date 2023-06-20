using Fabricdot.Core.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Queries;

[Dependency(ServiceLifetime.Singleton)]
public class QueryProcessor : IQueryProcessor
{
    private readonly ISender _sender;

    public QueryProcessor(ISender sender)
    {
        _sender = sender;
    }

    public async Task<TResult> ProcessAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default)
    {
        var ret = await _sender.Send(query, cancellationToken);
        return (TResult)ret!;
    }
}
