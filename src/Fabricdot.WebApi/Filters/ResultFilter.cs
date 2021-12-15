using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fabricdot.WebApi.Filters
{
    public class ResultFilter : IAsyncResultFilter
    {
        private readonly ISender _sender;

        public ResultFilter(ISender sender)
        {
            _sender = sender;
        }

        /// <inheritdoc />
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!ShouldHandleResult(context))
            {
                await next();
                return;
            }

            context.Result = await _sender.Send(new GetActionResultRequest(context));
            await next();
        }

        protected virtual bool ShouldHandleResult(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
                return true;

            return false;
        }
    }
}