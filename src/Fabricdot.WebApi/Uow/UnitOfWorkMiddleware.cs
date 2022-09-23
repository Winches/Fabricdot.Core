using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Uow;

[Dependency(ServiceLifetime.Transient)]
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
        var ignoredUrls = _options.IgnoredUrls;
        return path == null || ignoredUrls.All(v => !path.StartsWith(v));
    }
}