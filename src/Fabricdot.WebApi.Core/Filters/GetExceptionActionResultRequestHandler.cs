using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Common.Core.Exceptions;
using Fabricdot.Core.ExceptionHandling;
using Fabricdot.WebApi.Core.Endpoint;
using Fabricdot.WebApi.Core.Exceptions;
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

            //todo:remove code
            if (exception is WarningException warningException)
            {
                ret.Code = warningException.Code;
            }
            if (exception is ValidationException validationException)
            {
                ret.Data = validationException.Errors;
            }

            return Task.FromResult<IActionResult>(new ObjectResult(ret));
        }
    }
}