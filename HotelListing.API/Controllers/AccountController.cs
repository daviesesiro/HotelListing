using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;

    public AccountController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto dto)
    {
        var errors = await _authManager.Register(dto);

        if (errors.Any())
        {
            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var response = await _authManager.Login(dto);
        if (response == null) return Unauthorized();

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] AuthResponseDto request)
    {
        var response = await _authManager.VerifyRefreshToken(request);
        if (response == null) return Unauthorized();

        return Ok(response);
    }
}