using MediatR;

namespace Fabricdot.Domain.Core.Events
{
    public interface IDomainEventHandler<in TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
    }
}