using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Core.Tests
{
    public class ActionMiddlewareProvider
    {
        public RequestDelegate ExecutingAction { get; set; }

        public RequestDelegate ExecutedAction { get; set; }
    }
}