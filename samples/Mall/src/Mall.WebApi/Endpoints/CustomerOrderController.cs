using System.Threading.Tasks;
using Fabricdot.WebApi.Core.Endpoint;
using Mall.WebApi.Queries.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Mall.WebApi.Endpoints
{
    [Route("customer/order")]
    public class CustomerOrderController : EndPointBase
    {
        [HttpGet("amount")]
        public async Task<decimal> GetSpendingAmount([FromQuery] GetCustomerSpendingAmountQuery query)
        {
            return await Sender.Send(query);
        }
    }
}