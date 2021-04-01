using System;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Logging
{
    public static class ExceptionExtensions
    {
        public static LogLevel TryGetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
        }
    }
}