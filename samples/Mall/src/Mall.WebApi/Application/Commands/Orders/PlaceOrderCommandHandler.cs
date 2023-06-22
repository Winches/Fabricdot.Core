using Fabricdot.Infrastructure.Commands;
using Mall.Domain.Aggregates.OrderAggregate;
using Mall.Domain.Repositories;

namespace Mall.WebApi.Application.Commands.Orders;

public class PlaceOrderCommandHandler : CommandHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;

    public PlaceOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderService orderService)
    {
        _orderRepository = orderRepository;
        _orderService = orderService;
    }

    public override async Task<Guid> ExecuteAsync(
        PlaceOrderCommand command,
        CancellationToken cancellationToken)
    {
        var addressDto = command.ShippingAddress;
        var address = new Address(
            addressDto.Country,
            addressDto.State,
            addressDto.City,
            addressDto.Street);
        var order = _orderService.Create(address, command.CustomerId);
        command.OrderLines.ForEach(v => order.AddOrderLine(v.ProductId, v.Quantity, v.Price));

        await _orderRepository.AddAsync(order, cancellationToken);
        return order.Id;
    }
}
