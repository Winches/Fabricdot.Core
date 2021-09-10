using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Core.Uow
{
    public class UnitOfWorkMiddleware : IMiddleware
    {
        private readonly HttpUnitOfWorkOptions _options;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkMiddleware(
            IOptions<HttpUnitOfWorkOptions> options,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _options = options.Value;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (ShouldReserveUnitOfWork(context.Request))
            {
                using var uow = _unitOfWorkManager.Reserve(UnitOfWorkManager.RESERVATION_NAME);
                await next(context);
                if (uow.IsActive)//Prevent UOW be performed.
                    await uow.CommitChangesAsync(context.RequestAborted);
                return;
            }

            await next(context);
        }

        protected bool ShouldReserveUnitOfWork(HttpRequest request)
        {
            var path = request?.Path.Value;
            List<string> ignoredUrls = _options.IgnoredUrls;
            return path == null || ignoredUrls.All(v => !path.StartsWith(v));
        }
    }
}