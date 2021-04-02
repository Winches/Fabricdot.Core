using System;
using Ardalis.GuardClauses;
using Fabricdot.Core.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Validation
{
    public class ValidationFailedException : Exception, IHasNotification, IHasLogLevel
    {
        /// <inheritdoc />
        public Notification Notification { get; } = new Notification();

        /// <inheritdoc />
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        /// <inheritdoc />
        public ValidationFailedException([CanBeNull] string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ValidationFailedException([CanBeNull] string message, [CanBeNull] Exception innerException) : base(
            message, innerException)
        {
        }

        public ValidationFailedException([CanBeNull] string message, Notification notification) : base(message)
        {
            Guard.Against.Null(notification, nameof(notification));
            Notification = notification;
        }
    }
}