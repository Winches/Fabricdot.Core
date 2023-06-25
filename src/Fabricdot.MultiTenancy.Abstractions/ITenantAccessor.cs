namespace Fabricdot.MultiTenancy;

public interface ITenantAccessor
{
    ITenant? Tenant { get; }

    IDisposable Change(ITenant? tenant);
}
