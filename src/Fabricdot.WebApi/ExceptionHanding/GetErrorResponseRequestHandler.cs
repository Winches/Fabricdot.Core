using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Core.Validation;
using Fabricdot.WebApi.Endpoint;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.ExceptionHanding;

public class GetErrorResponseRequestHandler : IRequestHandler<GetErrorResponseRequest, ErrorResponse>
{
    private readonly ResponseOptions _options;

    public GetErrorResponseRequestHandler(IOptions<ResponseOptions> options)
    {
        _options = options.Value;
    }

    public Task<ErrorResponse> Handle(GetErrorResponseRequest request, CancellationToken cancellationToken)
    {
        var exception = request.Exception;
        var ret = new ErrorResponse(exception.Message, _options.DefaultErrorCode);

        if (exception is IHasErrorCode hasErrorCode)
            ret.Code = hasErrorCode.Code;

        if (exception is IHasNotification hasNotification)
        {
            ret.Data = hasNotification.Notification.Errors;
            ret.Code = _options.ValidationErrorCode;
        }

        return Task.FromResult(ret);
    }
}