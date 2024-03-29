using Fabricdot.Core.ExceptionHandling;
using MediatR;

namespace Fabricdot.Infrastructure.ExceptionHanding;

public abstract class ExceptionThrownEventHandlerBase :
    IExceptionThrownEventHandler<IExceptionThrownEvent>,
    INotificationHandler<ExceptionThrownEventNotification>
{
    /// <inheritdoc />
    public virtual Task HandleAsync(IExceptionThrownEvent @event)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    async Task INotificationHandler<ExceptionThrownEventNotification>.Handle(
        ExceptionThrownEventNotification notification,
        CancellationToken cancellationToken)
    {
        await HandleAsync(notification.Event);
    }
}
