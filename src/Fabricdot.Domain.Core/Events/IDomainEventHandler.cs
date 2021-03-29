using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Domain.Core.Events
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        /// <summary>
        ///     handle domain event
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}