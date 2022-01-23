using System;

namespace Fabricdot.MultiTenancy.Abstractions
{
    public interface ITenantAccessor
    {
        ITenant Tenant { get; }

        IDisposable Change(ITenant tenant);
    }
}