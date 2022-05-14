using System;

namespace Fabricdot.Domain.SharedKernel
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}