using System;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

public class Employee : FullAuditAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; private set; }
    public Guid? TenantId { get; private set; }

    public Employee(
        Guid id,
        string name,
        Guid? tenantId = null)
    {
        Guard.Against.NullOrEmpty(id, nameof(id));
        Guard.Against.NullOrEmpty(name, nameof(name));
        Name = name;
        Id = id;
        TenantId = tenantId;
    }

    private Employee()
    {
    }
}