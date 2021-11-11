using System;
using Fabricdot.Infrastructure.Core.Queries;

namespace Mall.WebApi.Queries.Orders
{
    public class GetOrderDetailsQuery : IQuery<OrderDetailsDto>
    {
        public Guid OrderId { get; }

        public GetOrderDetailsQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}