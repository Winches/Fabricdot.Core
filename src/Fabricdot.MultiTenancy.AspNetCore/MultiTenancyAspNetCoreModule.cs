using System;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.MultiTenancy.AspNetCore.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.AspNetCore
{
    public class MultiTenancyAspNetCoreModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            new MultiTenancyModule().Configure(services);

            services.Configure((Action<MultiTenancyOptions>)(opts =>
            {
                var resolveStrategies = opts.ResolveStrategies;
                resolveStrategies.Add(new QueryStringTenantResolveStrategy());
                resolveStrategies.Add(new RouteTenantResolveStrategy());
                resolveStrategies.Add(new HeaderTenantResolveStrategy());
                resolveStrategies.Add(new CookieTenantResolveStrategy());
            }));

            services.AddSingleton<IMultiTenancyExceptionHandler, MultiTenancyExceptionHandler>();
            services.AddTransient<MultiTenancyMiddleware>();
        }
    }
}