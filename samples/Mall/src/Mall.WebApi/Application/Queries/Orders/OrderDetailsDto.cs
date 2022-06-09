using System;
using System.Collections.Generic;
using AutoMapper;
using Mall.Domain.Aggregates.OrderAggregate;

namespace Mall.WebApi.Application.Queries.Orders;

[AutoMap(typeof(Order))]
public class OrderDetailsDto
{
    public Guid Id { get; set; }

    public decimal Total { get; set; }

    public DateTime OrderTime { get; set; }

    public OrderStatus OrderStatus { get; set; } = null!;

    public Address ShippingAddress { get; set; } = null!;

    public Guid CustomerId { get; set; }

    public ICollection<OrderLineDetailsDto> OrderLines { get; set; } = null!;
}
