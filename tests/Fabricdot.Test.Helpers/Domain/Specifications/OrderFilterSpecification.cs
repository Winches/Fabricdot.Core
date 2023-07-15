using Ardalis.Specification;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Test.Helpers.Domain.Specifications;

public sealed class OrderFilterSpecification : Specification<Order>
{
    public OrderFilterSpecification(CustomerId customerId)
    {
        Query.Where(v => v.CustomerId == customerId);
    }
}
