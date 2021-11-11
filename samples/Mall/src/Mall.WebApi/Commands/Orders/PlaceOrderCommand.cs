using System;
using System.ComponentModel.DataAnnotations;
using Fabricdot.Infrastructure.Core.Commands;

namespace Mall.WebApi.Commands.Orders
{
    public class PlaceOrderCommand : CommandBase<Guid>
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public AddressDto ShippingAddress { get; set; }

        [Required]
        [MinLength(1)]
        public OrderLineDto[] OrderLines { get; set; }
    }
}