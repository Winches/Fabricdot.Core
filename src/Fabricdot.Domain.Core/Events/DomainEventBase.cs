using System;
using Fabricdot.Domain.Core.SharedKernel;

namespace Fabricdot.Domain.Core.Events
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; }

        protected DomainEventBase()
        {
            OccurredOn = SystemClock.Now;
        }
    }
}