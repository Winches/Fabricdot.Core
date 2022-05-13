using System;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Infrastructure.Domain.Events
{
    [Dependency(ServiceLifetime.Scoped)]
    internal class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly ILogger<DomainEventPublisher> _logger;
        private readonly IPublisher _publisher;

        public DomainEventPublisher(
            ILogger<DomainEventPublisher> logger,
            IPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        public async Task PublishAsync(
            IDomainEvent @event,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Publish domain event:{EventName}", @event.GetType().PrettyPrint());
            var notification = new DomainEventNotification(@event);
            await _publisher.Publish(notification, cancellationToken);
        }
    }
}