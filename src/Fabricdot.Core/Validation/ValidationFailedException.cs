using Ardalis.GuardClauses;
using Fabricdot.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Validation;

public class ValidationFailedException : Exception, IHasNotification, IHasLogLevel
{
    /// <inheritdoc />
    public Notification Notification { get; } = new Notification();

    /// <inheritdoc />
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    public ValidationFailedException()
    {
    }

    public ValidationFailedException(string message) : base(message)
    {
    }

    public ValidationFailedException(
        string message,
        Exception innerException) : base(
        message, innerException)
    {
    }

    public ValidationFailedException(
        string message,
        Notification notification) : base(message)
    {
        Guard.Against.Null(notification, nameof(notification));
        Notification = notification;
    }
}
