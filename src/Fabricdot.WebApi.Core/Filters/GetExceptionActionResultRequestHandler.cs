using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Core.ExceptionHandling;
using Fabricdot.Core.Validation;
using Fabricdot.WebApi.Core.Endpoint;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Core.Filters
{
    public class
        GetExceptionActionResultRequestHandler : IRequestHandler<GetExceptionActionResultRequest, IActionResult>
    {
        /// <inheritdoc />
        public Task<IActionResult> Handle(GetExceptionActionResultRequest request, CancellationToken cancellationToken)
        {
            var exception = request.Exception;
            var ret = new Response<object>();
            ret.SetUnExcepted(exception.Message);

            if (exception is IHasErrorCode hasErrorCode)
            {
                ret.Code = hasErrorCode.Code;
            }

            if (exception is IHasNotification hasNotification)
            {
                ret.Data = hasNotification.Notification.Errors;
            }

            return Task.FromResult<IActionResult>(new ObjectResult(ret));
        }
    }
}