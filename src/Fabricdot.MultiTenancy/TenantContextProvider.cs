using System.Threading;
using System.Threading.Tasks;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    public class TenantContextProvider : ITenantContextProvider
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ITenantStore _tenantStore;

        public TenantContextProvider(
            ITenantResolver tenantResolver,
            ITenantStore tenantStore)
        {
            _tenantResolver = tenantResolver;
            _tenantStore = tenantStore;
        }

        public async Task<TenantContext> GetAsync(CancellationToken cancellationToken = default)
        {
            var res = await _tenantResolver.ResolveAsync();
            if (string.IsNullOrEmpty(res.Identifier))
                return null;

            var tenantContext = await _tenantStore.GetAsync(res.Identifier);
            if (tenantContext == null)
                throw new TenantNotFoundException("Tenant not found.");

            return tenantContext;
        }
    }
}