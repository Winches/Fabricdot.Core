using Fabricdot.Common.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Infrastructure.Core.Logging
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }
    }
}