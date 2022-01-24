using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy
{
    public class MultiTenancyModule : IModule
    {
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<ITenantResolver, TenantResolver>();
            services.AddTransient<ITenantContextProvider, TenantContextProvider>();

            services.AddSingleton<ITenantAccessor, DefaultTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
        }
    }
}