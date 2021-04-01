using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Common.Core.Exceptions;
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
            switch (exception)
            {
                //todo:remove code.
                case WarningException warningException:
                    ret.Code = warningException.Code;
                    ret.Message = warningException.Message;
                    if (exception is ValidationException validationException)
                        ret.Data = validationException.Errors;
                    break;
                default:
                    ret.SetUnExcepted(exception.Message);
                    break;
            }

            return Task.FromResult<IActionResult>(new ObjectResult(ret));
        }
    }
}