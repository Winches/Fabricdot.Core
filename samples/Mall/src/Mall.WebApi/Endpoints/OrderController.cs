using Fabricdot.WebApi.Endpoint;
using Mall.WebApi.Application.Commands.Orders;
using Mall.WebApi.Application.Queries.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Mall.WebApi.Endpoints;

public class OrderController : EndPointBase
{
    [HttpPost]
    public async Task<Guid> CreateAsync(PlaceOrderCommand command)
    {
        return await CommandBus.PublishAsync(command);
    }

    [HttpGet("{id}")]
    public async Task<OrderDetailsDto> GetDetails([FromRoute] Guid id)
    {
        return await QueryProcessor.ProcessAsync(new GetOrderDetailsQuery(id));
    }
}
