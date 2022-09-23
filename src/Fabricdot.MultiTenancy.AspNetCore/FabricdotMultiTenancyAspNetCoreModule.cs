using Fabricdot.Core.Modularity;
using Fabricdot.MultiTenancy.AspNetCore.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.AspNetCore;

[Requires(typeof(FabricdotMultiTenancyModule))]
[Exports]
public class FabricdotMultiTenancyAspNetCoreModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;

        services.Configure((Action<MultiTenancyOptions>)(opts =>
        {
            var resolveStrategies = opts.ResolveStrategies;
            resolveStrategies.Add(new QueryStringTenantResolveStrategy());
            resolveStrategies.Add(new RouteTenantResolveStrategy());
            resolveStrategies.Add(new HeaderTenantResolveStrategy());
            resolveStrategies.Add(new CookieTenantResolveStrategy());
        }));
    }
}