using MediatR;

namespace Fabricdot.Infrastructure.Queries;

public abstract class Query<TResult> : IQuery<TResult>, IRequest<TResult>
{
}