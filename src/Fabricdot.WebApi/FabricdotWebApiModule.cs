using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure;
using Fabricdot.WebApi.Tracing;
using Fabricdot.WebApi.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi
{
    [Requires(typeof(FabricdotInfrastructureModule))]
    [Exports]
    public class FabricdotWebApiModule : ModuleBase
    {
        public override void ConfigureServices(ConfigureServiceContext context)
        {
            var services = context.Services;

            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddOptions<CorrelationIdOptions>();
            services.AddOptions<HttpUnitOfWorkOptions>();
        }
    }
}