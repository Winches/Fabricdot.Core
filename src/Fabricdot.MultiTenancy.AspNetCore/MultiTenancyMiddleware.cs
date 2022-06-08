using System;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.AspNetCore;

[Dependency(ServiceLifetime.Transient)]
public class MultiTenancyMiddleware : IMiddleware
{
    private readonly ITenantContextProvider _tenantContextProvider;
    private readonly ITenantAccessor _tenantAccessor;
    private readonly IMultiTenancyExceptionHandler _multiTenancyExceptionHandler;

    public MultiTenancyMiddleware(
        ITenantContextProvider tenantContextProvider,
        ITenantAccessor tenantAccessor,
        IMultiTenancyExceptionHandler multiTenancyExceptionHandler)
    {
        _tenantContextProvider = tenantContextProvider;
        _tenantAccessor = tenantAccessor;
        _multiTenancyExceptionHandler = multiTenancyExceptionHandler;
    }

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        TenantContext? tenantContext;
        try
        {
            var uowMgr = context.RequestServices.GetService<IUnitOfWorkManager>();
            tenantContext = await _tenantContextProvider.GetAsync(context.RequestAborted);
        }
        catch (Exception ex)
        {
            await _multiTenancyExceptionHandler.HandleAsync(context, ex);
            return;
        }

        if (_tenantAccessor.Tenant?.Id != tenantContext?.Id)
        {
            using var scope = _tenantAccessor.Change(tenantContext);
            await next(context);
            return;
        }

        await next(context);
    }
}