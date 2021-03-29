using System;

namespace Fabricdot.Domain.Core.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}