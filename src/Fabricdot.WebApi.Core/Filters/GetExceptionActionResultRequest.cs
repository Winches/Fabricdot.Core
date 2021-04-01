using System;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Core.Filters
{
    public class GetExceptionActionResultRequest : IRequest<IActionResult>
    {
        public Exception Exception { get; }

        public GetExceptionActionResultRequest(Exception exception)
        {
            Guard.Against.Null(exception, nameof(exception));
            Exception = exception;
        }
    }
}