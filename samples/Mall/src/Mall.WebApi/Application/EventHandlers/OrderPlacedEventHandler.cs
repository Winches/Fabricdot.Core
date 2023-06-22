using Fabricdot.Domain.Events;
using Mall.Domain.Events;

namespace Mall.WebApi.Application.EventHandlers;

internal class OrderPlacedEventHandler : IDomainEventHandler<OrderPlacedEvent>
{
    private readonly ILogger<OrderPlacedEventHandler> _logger;

    public OrderPlacedEventHandler(ILogger<OrderPlacedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(
        OrderPlacedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var order = domainEvent.Entity;
        _logger.LogInformation("Do something after order placed.OrderId:{orderId}", order.Id);
        return Task.CompletedTask;
    }
}
