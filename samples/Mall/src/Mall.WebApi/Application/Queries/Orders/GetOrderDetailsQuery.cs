using System;
using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

public class GetOrderDetailsQuery : IQuery<OrderDetailsDto>
{
    public Guid OrderId { get; }

    public GetOrderDetailsQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}
