using Fabricdot.Domain.Events;
using Fabricdot.Infrastructure.Domain.Events;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Domain.Events;

public class DomainEventNotificationHandlerTests : IntegrationTestBase<InfrastructureTestModule>
{
    internal class OrderCreatedEventHandler1 : IDomainEventHandler<EntityCreatedEvent<Order>>
    {
        public static IDomainEvent Event { get; private set; }

        /// <inheritdoc />
        public Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken)
        {
            Event = domainEvent;
            return Task.CompletedTask;
        }
    }

    internal class OrderCreatedEventHandler2 : IDomainEventHandler<EntityCreatedEvent<Order>>
    {
        public static IDomainEvent Event { get; private set; }

        /// <inheritdoc />
        public Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken)
        {
            Event = domainEvent;
            return Task.CompletedTask;
        }
    }

    private readonly INotificationHandler<DomainEventNotification> _notificationHandler;

    /// <inheritdoc />
    public DomainEventNotificationHandlerTests()
    {
        _notificationHandler = ServiceProvider.GetRequiredService<INotificationHandler<DomainEventNotification>>();
    }

    [Fact]
    public async Task Handle_SubscribeMultipleEventHandlers_TriggerAllHandlers()
    {
        var @event = Create<EntityCreatedEvent<Order>>();
        await _notificationHandler.Handle(new DomainEventNotification(@event), default);

        OrderCreatedEventHandler1.Event.Should().Be(@event);
        OrderCreatedEventHandler2.Event.Should().Be(@event);
    }

    [Fact]
    public async Task Handle_NonSubscription_DoNothing()
    {
        var @event = Create<EntityRemovedEvent<Order>>();
        await _notificationHandler.Handle(new DomainEventNotification(@event), default);
    }
}