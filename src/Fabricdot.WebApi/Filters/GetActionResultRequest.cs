using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fabricdot.WebApi.Filters;

public class GetActionResultRequest : IRequest<IActionResult>
{
    public ResultExecutingContext Context { get; }

    public GetActionResultRequest(ResultExecutingContext context)
    {
        Guard.Against.Null(context, nameof(context));
        Context = context;
    }
}