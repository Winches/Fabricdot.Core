using MediatR;

namespace Fabricdot.Infrastructure.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}