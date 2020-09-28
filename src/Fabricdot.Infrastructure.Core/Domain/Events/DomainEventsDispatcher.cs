using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Common.Core.Enumerable;
using Fabricdot.Infrastructure.Core.Data;
using MediatR;

namespace Fabricdot.Infrastructure.Core.Domain.Events
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IEntityChangeTracker _entityChangeTracker;

        public DomainEventsDispatcher(IMediator mediator, IEntityChangeTracker entityChangeTracker)
        {
            _mediator = mediator;
            _entityChangeTracker = entityChangeTracker;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = _entityChangeTracker.Entries();

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent => { await _mediator.Publish(domainEvent); });

            await Task.WhenAll(tasks);
        }
    }
}