namespace Fabricdot.MultiTenancy.Abstractions;

// TODO：Implement the infrastructure
[Flags]
public enum TenantIsolation
{
    None = 0,
    Shared = 1,
    PerTenant = 2,
    Hybrid = Shared | PerTenant
}
