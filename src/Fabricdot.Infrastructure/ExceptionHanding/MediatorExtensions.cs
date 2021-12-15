using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.ExceptionHandling;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Infrastructure.ExceptionHanding
{
    public static class MediatorExtensions
    {
        public static async Task NotifyExceptionAsync(
            this IMediator mediator,
            IExceptionThrownEvent @event)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            var notification = new ExceptionThrownEventNotification(@event);
            await mediator.Publish(notification);
        }

        public static async Task NotifyExceptionAsync(
            this IMediator mediator,
            Exception exception,
            LogLevel? logLevel = null)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            var notification = new ExceptionThrownEventNotification(new ExceptionThrownEvent(exception, logLevel));
            await mediator.Publish(notification);
        }
    }
}