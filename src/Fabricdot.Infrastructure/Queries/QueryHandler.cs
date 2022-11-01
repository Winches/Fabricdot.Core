using MediatR;

namespace Fabricdot.Infrastructure.Queries;

public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>, IRequestHandler<TQuery, TResult> where TQuery : Query<TResult>
{
    public abstract Task<TResult> ExecuteAsync(
        TQuery query,
        CancellationToken cancellationToken);

    public async Task<TResult> Handle(
        TQuery request,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(request, cancellationToken);
    }
}