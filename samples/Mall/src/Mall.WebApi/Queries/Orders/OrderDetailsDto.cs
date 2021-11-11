using System;
using System.Collections.Generic;
using AutoMapper;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.WebApi.Queries.Orders
{
    [AutoMap(typeof(Order))]
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }

        public decimal Total { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Address ShippingAddress { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderLineDetailsDto> OrderLines { get; set; }
    }
}