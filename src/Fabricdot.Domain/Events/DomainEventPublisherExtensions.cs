using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Events;

public static class DomainEventPublisherExtensions
{
    public static async Task PublishAsync(
        this IDomainEventPublisher domainEventPublisher,
        IReadOnlyCollection<IHasDomainEvents> domainEntities,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(domainEventPublisher, nameof(domainEventPublisher));
        Guard.Against.Null(domainEntities, nameof(domainEntities));

        var domainEvents = domainEntities.Where(v => v.DomainEvents != null)
                                         .SelectMany(x => x.DomainEvents);

        //Task.WhenAll will cause concurrency issue
        foreach (var domainEvent in domainEvents)
            await domainEventPublisher.PublishAsync(domainEvent, cancellationToken);

        domainEntities.ForEach(v => v.ClearDomainEvents());
    }
}