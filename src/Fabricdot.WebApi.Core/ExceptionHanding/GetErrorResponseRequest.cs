using System;
using Ardalis.GuardClauses;
using Fabricdot.WebApi.Core.Endpoint;
using MediatR;

namespace Fabricdot.WebApi.Core.ExceptionHanding
{
    public class GetErrorResponseRequest : IRequest<ErrorResponse>
    {
        public Exception Exception { get; }

        public GetErrorResponseRequest(Exception exception)
        {
            Exception = Guard.Against.Null(exception, nameof(exception));
        }
    }
}