using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

public class GetCustomerSpendingAmountQuery : Query<decimal>
{
    public Guid CustomerId { get; set; }
}
