using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

internal class GetOrderDetailsQueryHandler : QueryHandler<GetOrderDetailsQuery, OrderDetailsDto>
{
    private readonly IOrderQueries _orderQueries;

    public GetOrderDetailsQueryHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }

    public override async Task<OrderDetailsDto> ExecuteAsync(
        GetOrderDetailsQuery query,
        CancellationToken cancellationToken)
    {
        return await _orderQueries.GetDetailsAsync(query.OrderId);
    }
}
