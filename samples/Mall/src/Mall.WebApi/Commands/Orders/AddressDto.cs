using System.ComponentModel.DataAnnotations;
using Mall.Domain.Shared.Constants;

namespace Mall.WebApi.Commands.Orders
{
    public class AddressDto
    {
        [Required]
        [StringLength(AddressConstant.CountryLength)]
        public string Country { get; set; }

        [Required]
        [StringLength(AddressConstant.StateLength)]
        public string State { get; set; }

        [Required]
        [StringLength(AddressConstant.CityLength)]
        public string City { get; set; }

        [Required]
        [StringLength(AddressConstant.StreetLength)]
        public string Street { get; set; }
    }
}