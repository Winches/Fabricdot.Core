using Ardalis.GuardClauses;
using MediatR;

namespace Fabricdot.Infrastructure.Domain.Events
{
    public class DomainEventNotification : INotification
    {
        public object Event { get; }

        public DomainEventNotification(object @event)
        {
            Guard.Against.Null(@event, nameof(@event));
            Event = @event;
        }
    }
}