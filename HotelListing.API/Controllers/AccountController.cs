using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
    {
        _authManager = authManager;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto dto)
    {
        _logger.LogInformation($"registration attempt for {dto.Email}");
        try
        {
            var errors = await _authManager.Register(dto);

            if (errors.Any())
            {
                foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error during registration for {dto.Email}");
            return Problem($"Something went wrong in the {nameof(Register)}. Please contact support", statusCode: 500);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation($"login attempt for {dto.Email}");
        try
        {
            var response = await _authManager.Login(dto);
            if (response == null) return Unauthorized();

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"something went wrong in {nameof(Login)}");
            return Problem($"Something went wrong in {nameof(Login)}", statusCode: 500);
        }
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