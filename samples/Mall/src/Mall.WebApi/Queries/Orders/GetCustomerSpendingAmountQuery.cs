using System;
using Fabricdot.Infrastructure.Core.Queries;

namespace Mall.WebApi.Queries.Orders
{
    public class GetCustomerSpendingAmountQuery : IQuery<decimal>
    {
        public Guid CustomerId { get; set; }
    }
}