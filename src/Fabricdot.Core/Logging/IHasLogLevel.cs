using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Logging;

public interface IHasLogLevel
{
    LogLevel LogLevel { get; }
}
