using Ardalis.GuardClauses;
using Fabricdot.Core.ExceptionHandling;
using MediatR;

namespace Fabricdot.Infrastructure.Core.ExceptionHanding
{
    //TODO:Implement event bus
    public class ExceptionThrownEventNotification : INotification
    {
        public IExceptionThrownEvent Event { get; }

        public ExceptionThrownEventNotification(IExceptionThrownEvent @event)
        {
            Guard.Against.Null(@event, nameof(@event));
            Event = @event;
        }
    }
}