using System;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.WebApi.Core.Endpoint;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Core.Filters
{
    [UsedImplicitly]
    public class GetActionResultRequestHandler : IRequestHandler<GetActionResultRequest, IActionResult>
    {
        private readonly ResultFilterOptions _options;

        public GetActionResultRequestHandler(IOptions<ResultFilterOptions> options)
        {
            _options = options.Value;
        }

        private static Task<IActionResult> GetEmptyResult() => Task.FromResult<IActionResult>(new ObjectResult(NullResponse.Null));

        /// <inheritdoc />
        public virtual Task<IActionResult> Handle(GetActionResultRequest request, CancellationToken cancellationToken)
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
                        return GetEmptyResult();

                    //instance of Response
                    var declaredType = objectResult.DeclaredType;
                    if (declaredType.IsGenericType && declaredType.GetGenericTypeDefinition() == typeof(Response<>))
                        break;

                    var ret = Activator.CreateInstance(typeof(Response<>).MakeGenericType(declaredType), resultValue);
                    return Task.FromResult<IActionResult>(new ObjectResult(ret));

                case EmptyResult _:
                    if (_options.IncludeEmptyResult)
                        return GetEmptyResult();
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