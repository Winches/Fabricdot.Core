namespace Fabricdot.MultiTenancy;

public interface ITenantResolver
{
    Task<TenantResolveResult> ResolveAsync();
}
