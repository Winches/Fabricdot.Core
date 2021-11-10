namespace Fabricdot.WebApi.Core.Endpoint
{

    public class ErrorResponse : Response<object>
    {
        public ErrorResponse(string message, int code) : base(message, code)
        {
            Success = false;
        }
    }
}