using System.Threading.Tasks;
using Fabricdot.Core.Security;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.Strategies
{
    public class PrincipalTenantResolveStrategy : ITenantResolveStrategy
    {
        public virtual int Priority => -1000;

        public Task<string> ResolveIdentifierAsync(TenantResolveContext context)
        {
            var principalAccessor = context.ServiceProvider.GetRequiredService<IPrincipalAccessor>();
            var principal = principalAccessor.Principal;
            if (principal?.Identity is not { IsAuthenticated: true })
                return Task.FromResult<string>(null);

            var tenantIdClaim = principal.FindFirst(v => v.Type == TenantClaimTypes.TenantId);

            return Task.FromResult(tenantIdClaim?.Value);
        }
    }
}