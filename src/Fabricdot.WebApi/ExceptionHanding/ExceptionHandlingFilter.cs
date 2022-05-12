using System;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.ExceptionHanding;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.ExceptionHanding
{
    [ServiceContract(typeof(ExceptionHandlingFilter))]
    [Dependency(ServiceLifetime.Scoped)]
    public class ExceptionHandlingFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;
        private readonly IMediator _mediator;

        public ExceptionHandlingFilter(
            ILogger<ExceptionHandlingFilter> logger,
            IMediator mediator)
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
                var res = await _mediator.Send(new GetErrorResponseRequest(exception));
                context.Result = new ObjectResult(res);
                await _mediator.NotifyExceptionAsync(exception);
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