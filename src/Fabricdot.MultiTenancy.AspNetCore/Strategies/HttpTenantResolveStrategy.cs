using System.Threading.Tasks;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.AspNetCore.Strategies
{
    public abstract class HttpTenantResolveStrategy : ITenantResolveStrategy
    {
        public virtual int Priority { get; protected set; }

        public virtual async Task<string?> ResolveIdentifierAsync(TenantResolveContext context)
        {
            var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            return await ResolveIdentifierAsync(httpContext);
        }

        protected abstract Task<string?> ResolveIdentifierAsync(HttpContext httpContext);
    }
}