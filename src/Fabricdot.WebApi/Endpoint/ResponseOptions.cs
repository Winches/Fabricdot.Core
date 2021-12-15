using System.Net;

namespace Fabricdot.WebApi.Endpoint
{
    public class ResponseOptions
    {
        public int DefaultErrorCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        public int ValidationErrorCode { get; set; } = (int)HttpStatusCode.BadRequest;
    }
}