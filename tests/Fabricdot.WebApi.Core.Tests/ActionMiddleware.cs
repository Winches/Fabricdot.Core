using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.WebApi.Core.Configuration;
using Fabricdot.WebApi.Core.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Core.Tests
{

    public class ActionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var actionMiddlewareProvider = context.RequestServices.GetRequiredService<ActionMiddlewareProvider>();
            var executingAction = actionMiddlewareProvider.ExecutingAction;
            var executedAction = actionMiddlewareProvider.ExecutedAction;

            if (executingAction != null)
                await executingAction(context);
            await next(context);
            if (executedAction != null)
                await executedAction(context);
        }
    }
}