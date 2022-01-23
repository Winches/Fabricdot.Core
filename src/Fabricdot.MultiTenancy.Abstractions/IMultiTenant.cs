using System;

namespace Fabricdot.MultiTenancy.Abstractions
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}