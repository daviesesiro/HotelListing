using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(ApiUserDto dto);
    Task<AuthResponseDto> Login(LoginDto dto);
    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
    Task<string> GenerateRefreshToken(ApiUser user);
}