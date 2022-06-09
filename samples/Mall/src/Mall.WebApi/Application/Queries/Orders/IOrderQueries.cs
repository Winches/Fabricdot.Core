using System;
using System.Threading.Tasks;

namespace Mall.WebApi.Application.Queries.Orders;

public interface IOrderQueries
{
    Task<OrderDetailsDto> GetDetailsAsync(Guid orderId);

    Task<decimal> GetSpendingAmount(Guid customerId);
}
