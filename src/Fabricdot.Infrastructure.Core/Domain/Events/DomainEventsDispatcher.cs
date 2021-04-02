using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.Core.Data;
using MediatR;

namespace Fabricdot.Infrastructure.Core.Domain.Events
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IPublisher _publisher;
        private readonly IEntityChangeTracker _entityChangeTracker;

        public DomainEventsDispatcher(IPublisher publisher, IEntityChangeTracker entityChangeTracker)
        {
            _publisher = publisher;
            _entityChangeTracker = entityChangeTracker;
        }

        public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = _entityChangeTracker.Entries()
                .Select(v => v.Entity)
                .Cast<IHasDomainEvents>()
                .ToArray();

            var domainEvents = domainEntities.Where(v => v.DomainEvents != null)
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            //Task.WhenAll will cause concurrency issue
            foreach (var domainEvent in domainEvents)
                //await _publisher.Publish(domainEvent, cancellationToken);
                await _publisher.Publish(new DomainEventNotification(domainEvent), cancellationToken);
        }
    }
}