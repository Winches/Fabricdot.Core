using System;
using Ardalis.Specification;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.Domain.Specifications
{
    public sealed class OrderWithLinesSpec : Specification<Order>
    {
        public OrderWithLinesSpec(Guid orderId)
        {
            Query.Include(v => v.OrderLines);
            Query.Where(v => v.Id == orderId);
        }
    }
}