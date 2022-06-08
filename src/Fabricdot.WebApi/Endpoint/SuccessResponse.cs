namespace Fabricdot.WebApi.Endpoint;

public class SuccessResponse : Response<object>
{
    public SuccessResponse(
        object? data,
        int code) : base(data)
    {
        Success = true;
        Code = code;
    }
}