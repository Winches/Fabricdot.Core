using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Infrastructure.ExceptionHanding;
using Fabricdot.Infrastructure.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.ExceptionHanding;

public class RequestExceptionThrownEventHandler : ExceptionThrownEventHandlerBase
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestExceptionThrownEventHandler(
        ILoggerFactory loggerFactory,
        ICorrelationIdAccessor correlationIdAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        _loggerFactory = loggerFactory;
        _correlationIdAccessor = correlationIdAccessor;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public override Task HandleAsync(IExceptionThrownEvent @event)
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
}
