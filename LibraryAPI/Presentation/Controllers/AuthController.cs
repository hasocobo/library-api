using System.Security.Claims;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Users;
using LibraryAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("users")]
    public async Task<ActionResult<UserDetails>> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var registeredUser = await _serviceManager.AuthService.RegisterUserAsync(userRegistrationDto);
        return Ok(registeredUser);
    }

    [HttpGet("users/{id}")]
    public async Task<ActionResult<UserDetails>> GetUserById(string id)
    {
        var userToReturn = await _serviceManager.AuthService.GetUserByIdAsync(id);
        return Ok(userToReturn);
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDetails>>> GetUsers()
    {
        var usersToReturn = await _serviceManager.AuthService.GetUsersAsync();

        return Ok(usersToReturn);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
    {
        if (!await _serviceManager.AuthService.ValidateUserAsync(userAuthenticationDto))
        {
            return Unauthorized();
        }

        var jwtToken = _serviceManager.AuthService.CreateTokenAsync();

        return Ok(jwtToken);
    }

    [HttpGet("/me")]
    public async Task<ActionResult<UserDetails>> GetCurrentUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _serviceManager.AuthService.GetUserByIdAsync(userId!);

        return Ok(user);
    }
}