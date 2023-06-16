using Fabricdot.Domain.Entities;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class OrderDetails : FullAuditEntity<Guid>
{
    public Guid? OrderId { get; private set; }

    public OrderDetails(Guid id)
    {
        Id = id;
    }

    private OrderDetails()
    {
    }
}
