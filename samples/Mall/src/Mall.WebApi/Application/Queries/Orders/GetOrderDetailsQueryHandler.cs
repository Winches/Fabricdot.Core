using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Queries;

namespace Mall.WebApi.Application.Queries.Orders;

internal class GetOrderDetailsQueryHandler : IQueryHandler<GetOrderDetailsQuery, OrderDetailsDto>
{
    private readonly IOrderQueries _orderQueries;

    public GetOrderDetailsQueryHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }

    public async Task<OrderDetailsDto> Handle(
        GetOrderDetailsQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderQueries.GetDetailsAsync(request.OrderId);
    }
}
