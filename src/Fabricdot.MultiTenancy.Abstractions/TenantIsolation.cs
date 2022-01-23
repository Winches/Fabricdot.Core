using System;

namespace Fabricdot.MultiTenancy.Abstractions
{
    // TODO：Implement the infrastructure
    [Flags]
    public enum TenantIsolation
    {
        Shared = 1,
        PerTenant = 2,
        Hybrid = Shared | PerTenant
    }
}