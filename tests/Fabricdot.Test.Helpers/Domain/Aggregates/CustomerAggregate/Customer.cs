using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

public class Customer : FullAuditAggregateRoot<CustomerId>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public Customer(
        CustomerId id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public Customer(
        CustomerId id,
        string name,
        Guid tenantId) : this(id, name)
    {
        TenantId = tenantId;
    }

    private Customer()
    {
    }
}
