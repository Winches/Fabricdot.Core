using System;
using AutoMapper;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.WebApi.Queries.Orders
{
    [AutoMap(typeof(OrderLine))]
    public class OrderLineDetailsDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}