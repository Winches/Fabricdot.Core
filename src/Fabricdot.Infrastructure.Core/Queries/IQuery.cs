using MediatR;

namespace Fabricdot.Infrastructure.Core.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}