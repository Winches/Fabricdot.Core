using System.Threading.Tasks;
using Fabricdot.Common.Core.Exceptions;
using Fabricdot.WebApi.Core.Endpoint;
using Fabricdot.WebApi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.Core.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
                return;

            await HandleException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor) return true;

            return false;
        }

        protected virtual Task HandleException(ExceptionContext context)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress;
            var path = context.HttpContext.Request.Path;
            var exception = context.Exception;
            var ret = new Response<object>();

            switch (exception)
            {
                case WarningException warningException:
                    ret.Code = warningException.Code;
                    ret.Message = warningException.Message;
                    if (exception is ValidationException validationException) ret.Data = validationException.Errors;

                    _logger.LogWarning($"ip={ip}, path={path}, error={warningException.Message}");
                    break;

                default:
                    ret.SetUnExcepted(exception.Message);
                    _logger.LogError(exception, $"ip={ip}, path={path}, error={exception.Message}");
                    break;
            }

            context.Result = new ObjectResult(ret);
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}