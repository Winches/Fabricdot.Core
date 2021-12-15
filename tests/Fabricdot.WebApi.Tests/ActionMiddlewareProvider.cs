using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Tests
{
    public class ActionMiddlewareProvider
    {
        public RequestDelegate ExecutingAction { get; set; }

        public RequestDelegate ExecutedAction { get; set; }
    }
}