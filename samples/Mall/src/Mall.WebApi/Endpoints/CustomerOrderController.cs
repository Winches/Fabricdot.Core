using Fabricdot.WebApi.Endpoint;
using Mall.WebApi.Application.Queries.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Mall.WebApi.Endpoints;

[Route("customer/order")]
public class CustomerOrderController : EndPointBase
{
    [HttpGet("amount")]
    public async Task<decimal> GetSpendingAmount([FromQuery] GetCustomerSpendingAmountQuery query)
    {
        return await QueryProcessor.ProcessAsync(query);
    }
}
