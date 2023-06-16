using Fabricdot.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Domain.Events;

internal class DomainEventNotificationHandler : INotificationHandler<DomainEventNotification>
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventNotificationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task Handle(DomainEventNotification notification, CancellationToken cancellationToken)
    {
        var @event = notification.Event;
        var eventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
        var eventHandlers = _serviceProvider.GetServices(eventHandlerType);

        foreach (var eventHandler in eventHandlers)
        {
            var method = eventHandler?.GetType()
                .GetMethod(nameof(IDomainEventHandler<DomainEventBase>.HandleAsync),
                    new[] { @event.GetType(), typeof(CancellationToken) });
            var task = (Task?)method?.Invoke(eventHandler, new[] { @event, cancellationToken }) ?? Task.CompletedTask;
            await task.ConfigureAwait(false);
        }
    }
}
