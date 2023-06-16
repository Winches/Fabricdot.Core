namespace Fabricdot.Core.Modularity;

public class ModularityException : Exception
{
    public ModularityException()
    {
    }

    public ModularityException(string message) : base(message)
    {
    }

    public ModularityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
