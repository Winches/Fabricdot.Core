using System;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.MultiTenancy.Tests.Entities;

internal class MultiTenantEmployee : FullAuditAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public MultiTenantEmployee(string name) : this()
    {
        Name = name;
    }

    public MultiTenantEmployee(
        string name,
        Guid? tenantId) : this()
    {
        Name = name;
        TenantId = tenantId;
    }

    public MultiTenantEmployee()
    {
        Id = Guid.NewGuid();
    }
}