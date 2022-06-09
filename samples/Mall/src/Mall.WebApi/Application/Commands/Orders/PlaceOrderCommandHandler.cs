using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Commands;
using Mall.Domain.Aggregates.OrderAggregate;
using Mall.Domain.Repositories;

namespace Mall.WebApi.Application.Commands.Orders;

public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, Guid>
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

    public async Task<Guid> Handle(
        PlaceOrderCommand request,
        CancellationToken cancellationToken)
    {
        var addressDto = request.ShippingAddress;
        var address = new Address(
            addressDto.Country,
            addressDto.State,
            addressDto.City,
            addressDto.Street);
        var order = _orderService.Create(address, request.CustomerId);
        request.OrderLines.ForEach(v => order.AddOrderLine(v.ProductId, v.Quantity, v.Price));

        await _orderRepository.AddAsync(order);
        return order.Id;
    }
}
