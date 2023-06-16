namespace Fabricdot.Domain.Events;

public interface IDomainEventPublisher
{
    Task PublishAsync(
        IDomainEvent @event,
        CancellationToken cancellationToken = default);
}
