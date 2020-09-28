using System;
using MediatR;

namespace Fabricdot.Domain.Core.Events
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}