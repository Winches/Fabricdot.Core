using System;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.Data;
using Mall.Domain.Aggregates.OrderAggregate;
using Mall.Domain.Specifications;

namespace Mall.WebApi.Application.Queries.Orders;

internal class OrderQueries : IOrderQueries, ITransientDependency
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private readonly IReadOnlyRepository<Order, Guid> _orderRepository;

    private readonly IMapper _mapper;

    public OrderQueries(
        ISqlConnectionFactory sqlConnectionFactory,
        IReadOnlyRepository<Order, Guid> orderRepository,
        IMapper mapper)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<decimal> GetSpendingAmount(Guid customerId)
    {
        var dbConnection = _sqlConnectionFactory.GetOpenConnection();
        const string cmd = @"
                SELECT ISNULL(SUM(Total),0)
                FROM Orders t
                WHERE t.IsDeleted = 0
                    AND t.CustomerId = @CustomerId";
        var param = new
        {
            customerId
        };

        return await dbConnection.QuerySingleAsync<decimal>(cmd, param);
    }

    public async Task<OrderDetailsDto> GetDetailsAsync(Guid orderId)
    {
        var spec = new OrderWithLinesSpec(orderId);
        var order = await _orderRepository.GetAsync(spec);
        return _mapper.Map<OrderDetailsDto>(order);
    }
}
