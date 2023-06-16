using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.ExceptionHandling;

public interface IExceptionThrownEvent
{
    Exception Exception { get; }

    LogLevel? LogLevel { get; }
}
