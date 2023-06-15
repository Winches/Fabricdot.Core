using Fabricdot.WebApi.Endpoint;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Filters;

[UsedImplicitly]
public class GetActionResultRequestHandler : IRequestHandler<GetActionResultRequest, IActionResult>
{
    protected static Type ResponseType => typeof(Response<>);
    protected ResponseOptions Options { get; }

    public GetActionResultRequestHandler(IOptions<ResponseOptions> options)
    {
        Options = options.Value;
    }

    /// <inheritdoc />
    public virtual Task<IActionResult> Handle(GetActionResultRequest request, CancellationToken cancellationToken)
    {
        var context = request.Context;
        var originalResult = context.Result;

        var ret = new SuccessResponse(null, Options.SuccessCode);

        switch (originalResult)
        {
            //OkObjectResult, NotFoundObjectResult,BadRequestObjectResult CreatedResult
            case ObjectResult objectResult:
                var resultValue = objectResult.Value;
                if (originalResult is BadRequestObjectResult || originalResult is NotFoundObjectResult)
                    break;
                //instance of Response
                if (objectResult.DeclaredType?.IsAssignableToGenericType(ResponseType) ?? false)
                    break;

                if (resultValue is ProblemDetails problem)
                {
                    ret.Code = problem.Status ?? Options.DefaultErrorCode;
                    ret.Message = problem.Title;
                    ret.Success = false;
                    ret.Data = problem.Detail;
                }
                else
                {
                    ret.Data = resultValue;
                }
                return Task.FromResult<IActionResult>(new ObjectResult(ret));

            case EmptyResult:
                return Task.FromResult<IActionResult>(new ObjectResult(ret));
        }

        return Task.FromResult(originalResult);
    }
}
