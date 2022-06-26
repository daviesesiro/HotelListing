using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;

public class LoginDto
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public string Password { get; set; }
}