using System;
using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

public class GetCustomerSpendingAmountQuery : IQuery<decimal>
{
    public Guid CustomerId { get; set; }
}
