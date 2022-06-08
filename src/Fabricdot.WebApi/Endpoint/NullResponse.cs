namespace Fabricdot.WebApi.Endpoint;

public sealed class NullResponse : Response<object>
{
    public static NullResponse Null => new();

    private NullResponse()
    {
    }
}