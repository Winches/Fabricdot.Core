using System;
using System.Threading.Tasks;

namespace Mall.WebApi.Queries.Orders
{
    public interface IOrderQueries
    {
        Task<OrderDetailsDto> GetDetailsAsync(Guid orderId);

        Task<decimal> GetSpendingAmount(Guid customerId);
    }
}