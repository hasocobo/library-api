using System.Security.Claims;
using System.Text.Json;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Users;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpPost("users")]
    public async Task<ActionResult<UserDetails>> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var registeredUser = await _serviceManager.AuthService.RegisterUserAsync(userRegistrationDto);
        return Ok(registeredUser);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("users/{id}")]
    public async Task<ActionResult<UserDetails>> GetUserById(string id)
    {
        var userToReturn = await _serviceManager.AuthService.GetUserByIdAsync(id);
        return Ok(userToReturn);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDetails>>> GetUsers([FromQuery] QueryParameters queryParameters)
    {
        var pagedResponse = await _serviceManager.AuthService.GetUsersAsync(queryParameters);
        var usersToReturn = pagedResponse.Items;
        var paginationMetadata = new
        {
            PageNumber = pagedResponse.PageNumber,
            TotalPages = pagedResponse.TotalPages,
            PageSize = pagedResponse.PageSize,
            TotalCount = pagedResponse.TotalCount
        };
        
        Response.Headers["LibraryApi-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        
        return Ok(usersToReturn);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
    {
        if (!await _serviceManager.AuthService.ValidateUserAsync(userAuthenticationDto))
        {
            return Unauthorized();
        }

        var (jwtToken, userDetails) = await _serviceManager.AuthService.LoginAsync();

        return Ok(new { jwtToken, userDetails });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("users/{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        await _serviceManager.AuthService.DeleteUserAsync(userId);
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("users/{userId}")]
    public async Task<ActionResult> ChangeUserInformation(string userId, UserUpdateDto userUpdateDto)
    {
        await _serviceManager.AuthService.ChangeUserInformationAsync(userId, userUpdateDto);
        return Ok();
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("/me")]
    public async Task<ActionResult<UserDetails>> GetCurrentUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _serviceManager.AuthService.GetUserByIdAsync(userId!);

        return Ok(user);
    }
}