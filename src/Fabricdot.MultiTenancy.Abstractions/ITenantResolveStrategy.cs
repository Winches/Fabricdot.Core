namespace Fabricdot.MultiTenancy;

public interface ITenantResolveStrategy
{
    int Priority { get; }

    Task<string?> ResolveIdentifierAsync(TenantResolveContext context);
}
