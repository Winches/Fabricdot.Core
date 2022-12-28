using System.ComponentModel.DataAnnotations;
using Fabricdot.Infrastructure.Commands;

namespace Mall.WebApi.Application.Commands.Orders;

public class PlaceOrderCommand : Command<Guid>
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public AddressDto ShippingAddress { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public OrderLineDto[] OrderLines { get; set; } = null!;
}
