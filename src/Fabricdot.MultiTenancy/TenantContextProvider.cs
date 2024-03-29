using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy;

[Dependency(ServiceLifetime.Transient)]
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

    public async Task<TenantContext?> GetAsync(CancellationToken cancellationToken = default)
    {
        var res = await _tenantResolver.ResolveAsync();
        if (string.IsNullOrEmpty(res.Identifier))
            return null;

        var tenantContext = await _tenantStore.GetAsync(res.Identifier, cancellationToken);
        return tenantContext ?? throw new TenantNotFoundException("Tenant not found.");
    }
}
