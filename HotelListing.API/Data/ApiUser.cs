using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data;

public class ApiUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}