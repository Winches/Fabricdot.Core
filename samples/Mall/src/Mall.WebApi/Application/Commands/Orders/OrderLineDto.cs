using System;
using System.ComponentModel.DataAnnotations;

namespace Mall.WebApi.Application.Commands.Orders;

public class OrderLineDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }
}
