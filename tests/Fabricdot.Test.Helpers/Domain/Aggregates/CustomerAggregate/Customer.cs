using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

public class Customer : FullAuditAggregateRoot<string>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public Customer(
        Guid id,
        string name)
    {
        Id = id.ToString();
        Name = name;
    }

    public Customer(
        Guid id,
        string name,
        Guid tenantId) : this(id, name)
    {
        TenantId = tenantId;
    }

    private Customer()
    {
    }
}