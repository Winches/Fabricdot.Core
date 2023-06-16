using Fabricdot.Core.ExceptionHandling;

namespace Fabricdot.Infrastructure.Commands;

public class CommandException : Exception, IHasErrorCode
{
    /// <inheritdoc />
    public int Code { get; }

    public CommandException(string message) : base(message)
    {
    }

    public CommandException(string message, int code) : base(message)
    {
        Code = code;
    }

    public CommandException(
        string message,
        int code,
        Exception exception) : base(message, exception)
    {
        Code = code;
    }
}
