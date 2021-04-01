using System;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.WebApi.Core.Endpoint;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Core.Filters
{
    public class GetActionResultRequestHandler : IRequestHandler<GetActionResultRequest, IActionResult>
    {
        /// <inheritdoc />
        public Task<IActionResult> Handle(GetActionResultRequest request, CancellationToken cancellationToken)
        {
            var context = request.Context;
            var originalResult = context.Result;

            switch (originalResult)
            {
                //OkObjectResult, NotFoundObjectResult,BadRequestObjectResult CreatedResult
                case ObjectResult objectResult:
                    var resultValue = objectResult.Value;
                    if (originalResult is BadRequestObjectResult || originalResult is NotFoundObjectResult)
                        break;

                    if (resultValue == null)
                        return Task.FromResult<IActionResult>(new ObjectResult(NullResponse.Null));
                    //instance of Response
                    var declaredType = objectResult.DeclaredType;
                    if (declaredType.IsGenericType && declaredType.GetGenericTypeDefinition() == typeof(Response<>))
                        break;

                    var ret = Activator.CreateInstance(typeof(Response<>).MakeGenericType(declaredType), resultValue);
                    return Task.FromResult<IActionResult>(new ObjectResult(ret));

                case EmptyResult _:
                    break;

                case ContentResult _:
                    break;

                //OKResult, NoContentResult, UnauthorizedResult, NotFoundResult, BadRequestResult
                case StatusCodeResult _:
                    break;
            }

            return Task.FromResult(originalResult);
        }
    }
}