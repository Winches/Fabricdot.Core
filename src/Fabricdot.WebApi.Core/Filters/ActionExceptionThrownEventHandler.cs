using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Infrastructure.Core.Tracing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.Core.Filters
{
    public class ActionExceptionThrownEventHandler :
        IExceptionThrownEventHandler<ActionExceptionThrownEvent>,
        INotificationHandler<ActionExceptionThrownEvent>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ICorrelationIdAccessor _correlationIdAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionExceptionThrownEventHandler(
            ILoggerFactory loggerFactory,
            ICorrelationIdAccessor correlationIdAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            _loggerFactory = loggerFactory;
            _correlationIdAccessor = correlationIdAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public Task HandleAsync(ActionExceptionThrownEvent @event)
        {
            var exception = @event.Exception;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var request = httpContext.Request;
                var route = $"{request.Method}:{request.Path}";
                var correlationId = _correlationIdAccessor.CorrelationId;

                var logger = _loggerFactory.CreateLogger("Fabricdot.ExceptionHandling");
                const string message = "CorrelationId:\"{CorrelationId}\", Route:\"{Route}\" :{Message}";
                logger.Log(@event.LogLevel ?? LogLevel.Error, exception, message, correlationId, route,
                    exception.Message);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task Handle(ActionExceptionThrownEvent notification, CancellationToken cancellationToken)
        {
            await HandleAsync(notification);
        }
    }
}