namespace Fabricdot.MultiTenancy;

public interface ITenantContextProvider
{
    Task<TenantContext?> GetAsync(CancellationToken cancellationToken = default);
}
