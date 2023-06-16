namespace Fabricdot.WebApi.Exceptions;

public class EndPointException : Exception
{
    public EndPointException()
    {
    }

    public EndPointException(string message) : base(message)
    {
    }

    public EndPointException(
        string message,
        Exception exception) : base(message, exception)
    {
    }
}
