using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.API.Repository;

public class AuthManager : IAuthManager
{
    private const string _loginProvier = "HotelListingApi";
    private const string _refreshTokenName = "RefreshToken";
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto dto)
    {
        var user = _mapper.Map<ApiUser>(dto);
        user.UserName = dto.Email;

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded) await _userManager.AddToRoleAsync(user, "USER");

        return result.Errors;
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        var isValidUser = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (user == null || isValidUser == false) return null;

        var token = await GenerateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            RefreshToken = await GenerateRefreshToken(user)
        };
    }

    public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenContent = tokenHandler.ReadJwtToken(request.Token);
        var username = tokenContent.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(username);

        if (user == null || !user.Id.Equals(request.UserId)) return null;

        var isValidRefreshToken =
            await _userManager.VerifyUserTokenAsync(user, _loginProvier, _refreshTokenName, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                RefreshToken = await GenerateRefreshToken(user)
            };
        }

        await _userManager.UpdateSecurityStampAsync(user);
        return null;
    }

    public async Task<string> GenerateRefreshToken(ApiUser user)
    {
        await _userManager.RemoveAuthenticationTokenAsync(user, _loginProvier, _refreshTokenName);
        var refreshToken = await _userManager.GenerateUserTokenAsync(user, _loginProvier, _refreshTokenName);
        await _userManager.SetAuthenticationTokenAsync(user, _loginProvier, _refreshTokenName, refreshToken);

        return refreshToken;
    }

    private async Task<string> GenerateToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roleClaims = (await _userManager.GetRolesAsync(user))
            .Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("uid", user.Id.ToString())
        }.Union(userClaims).Union(roleClaims);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:DurationInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}