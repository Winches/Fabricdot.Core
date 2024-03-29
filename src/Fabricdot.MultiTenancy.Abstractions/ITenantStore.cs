namespace Fabricdot.MultiTenancy;

public interface ITenantStore<TTenant> where TTenant : ITenant
{
    Task<TTenant> GetAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}

public interface ITenantStore : ITenantStore<TenantContext>
{
}
