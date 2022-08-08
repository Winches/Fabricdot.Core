using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests;

[ServiceContract(typeof(ActionMiddleware))]
public class ActionMiddleware : IMiddleware, ITransientDependency
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