using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Domain.Events;

public abstract class DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; }

    protected DomainEventBase()
    {
        OccurredOn = SystemClock.Now;
    }
}
