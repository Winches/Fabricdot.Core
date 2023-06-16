using Ardalis.GuardClauses;
using Fabricdot.WebApi.Endpoint;
using MediatR;

namespace Fabricdot.WebApi.ExceptionHanding;

public class GetErrorResponseRequest : IRequest<ErrorResponse>
{
    public Exception Exception { get; }

    public GetErrorResponseRequest(Exception exception)
    {
        Exception = Guard.Against.Null(exception, nameof(exception));
    }
}
