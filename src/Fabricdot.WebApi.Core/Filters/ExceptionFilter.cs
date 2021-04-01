using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.Core.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly IMediator _mediator;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
                return;

            await HandleException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            return context.ActionDescriptor switch
            {
                ControllerActionDescriptor _ => true,
                _ => false
            };
        }

        protected virtual async Task HandleException(ExceptionContext context)
        {
            var exception = context.Exception;
            try
            {
                context.Result = await _mediator.Send(new GetExceptionActionResultRequest(exception));
                await _mediator.Publish(new ActionExceptionThrownEvent(exception));
                context.ExceptionHandled = true;
            }
            catch (Exception ex)
            {
                const string message = "Not captured Exception :{Message}, Reason: {Reason}";
                _logger.LogError(ex, message, exception.Message, ex.Message);
                throw;
            }
        }
    }
}