namespace HotelListing.API.Models.Users;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}