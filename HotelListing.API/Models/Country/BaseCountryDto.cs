using System.ComponentModel.DataAnnotations;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country
{
    public abstract class BaseCountryDto
    {
        [Required] public string Name { get; set; }
        [Required] public string ShortName { get; set; }
    }
}