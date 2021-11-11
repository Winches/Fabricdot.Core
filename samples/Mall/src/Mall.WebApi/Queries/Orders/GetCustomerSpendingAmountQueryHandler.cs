using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Queries;

namespace Mall.WebApi.Queries.Orders
{
    internal class GetCustomerSpendingAmountQueryHandler : IQueryHandler<GetCustomerSpendingAmountQuery, decimal>
    {
        private readonly IOrderQueries _orderQueries;

        public GetCustomerSpendingAmountQueryHandler(IOrderQueries orderQueries)
        {
            _orderQueries = orderQueries;
        }

        public Task<decimal> Handle(
            GetCustomerSpendingAmountQuery request,
            CancellationToken cancellationToken)
        {
            return _orderQueries.GetSpendingAmount(request.CustomerId);
        }
    }
}