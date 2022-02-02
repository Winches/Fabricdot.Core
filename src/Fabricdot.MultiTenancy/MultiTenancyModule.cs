using Fabricdot.Domain.Internal;
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

            services.AddSingleton<ITenantAccessor>(DefaultTenantAccessor.Instance);
            services.AddTransient<ICurrentTenant, CurrentTenant>();

            EntityInitializer.Instance.Add<MultiTenantInitializer>();
        }
    }
}