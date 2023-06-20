using Fabricdot.Core.ExceptionHandling;

namespace Fabricdot.Domain.SharedKernel;

public class DomainException : Exception, IHasErrorCode
{
    /// <inheritdoc />
    public int Code { get; }

    public DomainException()
    {
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(
        string message,
        int code) : base(message)
    {
        Code = code;
    }

    public DomainException(
        string? message,
        Exception? innerException) : base(message, innerException)
    {
    }

    public DomainException(
        string message,
        int code,
        Exception exception) : base(message, exception)
    {
        Code = code;
    }
}
