﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using MediatR;

namespace Fabricdot.Infrastructure.Core.Domain.Events
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IPublisher _publisher;

        public DomainEventsDispatcher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        /// <inheritdoc />
        public async Task DispatchEventsAsync(
            ICollection<EntityChangeInfo> changeInfos,
            CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(changeInfos, nameof(changeInfos));
            var domainEntities = changeInfos.Select(v => v.Entity)
                .OfType<IHasDomainEvents>()
                .ToArray();

            var domainEvents = domainEntities.Where(v => v.DomainEvents != null)
                .SelectMany(x => x.DomainEvents)
                .ToList();

            //Task.WhenAll will cause concurrency issue
            foreach (var domainEvent in domainEvents)
                await _publisher.Publish(new DomainEventNotification(domainEvent), cancellationToken);
        }
    }
}