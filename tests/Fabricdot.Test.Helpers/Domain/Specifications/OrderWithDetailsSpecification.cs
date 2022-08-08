using Ardalis.Specification;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Test.Helpers.Domain.Specifications;

public sealed class OrderWithDetailsSpecification : Specification<Order>
{
    public OrderWithDetailsSpecification(Guid orderId)
    {
        Query.Where(v => v.Id == orderId);
        IncludeDetails();
    }

    public OrderWithDetailsSpecification()
    {
        IncludeDetails();
    }

    private void IncludeDetails()
    {
        Query.Include(v => v.OrderLines).Include(v => v.Details);
    }
}