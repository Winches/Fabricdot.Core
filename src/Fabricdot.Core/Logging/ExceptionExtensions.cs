using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Logging;

public static class ExceptionExtensions
{
    public static LogLevel TryGetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
    {
        return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
    }
}
