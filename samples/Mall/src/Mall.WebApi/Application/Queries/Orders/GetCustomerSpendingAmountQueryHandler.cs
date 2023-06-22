using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

internal class GetCustomerSpendingAmountQueryHandler : QueryHandler<GetCustomerSpendingAmountQuery, decimal>
{
    private readonly IOrderQueries _orderQueries;

    public GetCustomerSpendingAmountQueryHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }

    public override Task<decimal> ExecuteAsync(
        GetCustomerSpendingAmountQuery query,
        CancellationToken cancellationToken)
    {
        return _orderQueries.GetSpendingAmount(query.CustomerId);
    }
}
