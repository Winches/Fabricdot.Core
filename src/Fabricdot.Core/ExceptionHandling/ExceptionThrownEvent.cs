using Ardalis.GuardClauses;
using Fabricdot.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.ExceptionHandling;

public class ExceptionThrownEvent : IExceptionThrownEvent
{
    /// <inheritdoc />
    public Exception Exception { get; }

    /// <inheritdoc />
    public LogLevel? LogLevel { get; }

    public ExceptionThrownEvent(Exception exception, LogLevel? logLevel)
    {
        Guard.Against.Null(exception, nameof(exception));
        Exception = exception;
        LogLevel = logLevel ?? exception.TryGetLogLevel();
    }
}
