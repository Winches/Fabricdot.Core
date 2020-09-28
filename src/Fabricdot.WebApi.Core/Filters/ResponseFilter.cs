using System;
using System.Threading.Tasks;
using Fabricdot.WebApi.Core.Endpoint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fabricdot.WebApi.Core.Filters
{
    public class ResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!ShouldHandleResult(context))
            {
                await next();
                return;
            }

            HandleResult(context);
            await next();
        }

        protected virtual bool ShouldHandleResult(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor) return true;

            return false;
        }

        private static void HandleResult(ResultExecutingContext context)
        {
            var res = context.Result;
            switch (res)
            {
                case ObjectResult objectResult
                    : //OkObjectResult, NotFoundObjectResult,BadRequestObjectResult CreatedResult
                    if (res is BadRequestObjectResult || res is NotFoundObjectResult) return;

                    if (objectResult.Value == null)
                    {
                        context.Result = new ObjectResult(NullResponse.Null);
                    }
                    else
                    {
                        //instance of Response
                        var declaredType = objectResult.DeclaredType;
                        if (declaredType.IsGenericType && declaredType.GetGenericTypeDefinition() == typeof(Response<>))
                            return;
                        var ret = Activator.CreateInstance(typeof(Response<>).MakeGenericType(declaredType),
                            objectResult.Value);
                        context.Result = new ObjectResult(ret);
                    }

                    break;

                case EmptyResult _:
                    context.Result = new ObjectResult(NullResponse.Null);
                    break;

                case ContentResult _:
                    break;

                case StatusCodeResult _:
                    //OKResult, NoContentResult, UnauthorizedResult, NotFoundResult, BadRequestResult
                    break;
            }
        }
    }
}