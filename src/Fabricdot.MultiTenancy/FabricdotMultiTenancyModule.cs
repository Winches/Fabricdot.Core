using Fabricdot.Core.Modularity;
using Fabricdot.Domain.Internal;
using Fabricdot.Infrastructure;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy
{
    [Requires(typeof(FabricdotInfrastructureModule))]
    [Requires(typeof(FabricdotMultiTenancyAbstractionModule))]
    [Exports]
    public class FabricdotMultiTenancyModule : ModuleBase
    {
        public override void ConfigureServices(ConfigureServiceContext context)
        {
            var services = context.Services;

            services.AddSingleton<ITenantAccessor>(DefaultTenantAccessor.Instance);
            EntityInitializer.Instance.Add<MultiTenantInitializer>();
        }
    }
}