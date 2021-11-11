using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Queries;

namespace Mall.WebApi.Queries.Orders
{
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
}