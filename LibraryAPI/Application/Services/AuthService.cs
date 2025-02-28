﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Users;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Application.Services;

public class AuthService : IAuthService
{
    private ApplicationUser? _user;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ILogger<AuthService> logger, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<UserDetails> GetUserByIdAsync(string userId)
    {
        _logger.LogInformation($"Getting user with id: {userId}");
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User", Guid.Parse(userId));

        _logger.LogInformation($"Returning user details");
        var userDetails = new UserDetails
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        return userDetails;
    }

    public async Task<PagedResponse<UserDetails>> GetUsersAsync(QueryParameters queryParameters)
    {
        _logger.LogInformation($"Getting all users");
        var query = _userManager.Users;

        if (!String.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var keywords = queryParameters.SearchTerm
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().ToLower()).ToArray();

            query = query.Where(user => keywords.Any(keyword =>
                user.FirstName.ToLower().Contains(keyword) || user.LastName.ToLower().Contains(keyword) ||
                (user.Email != null && user.Email.ToLower().Contains(keyword)) ||
                (user.UserName != null && user.UserName.ToLower().Contains(keyword)) ||
                user.Id.ToLower().Contains(keyword)));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();


        _logger.LogInformation($"Returning all {users.Count} users");

        var usersToReturnTasks = users.Select(async user => new UserDetails
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = await _userManager.GetRolesAsync(user)
        });

        var usersToReturn = await Task.WhenAll(usersToReturnTasks);
        
        var pagedResponse = new PagedResponse<UserDetails>
        {
            TotalCount = totalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / queryParameters.PageSize),
            Items = usersToReturn
        };
        return pagedResponse;
    }

    public async Task<UserDetails> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        _logger.LogInformation($"Registering new user with username {userRegistrationDto.UserName}");

        var newUser = new ApplicationUser
        {
            Email = userRegistrationDto.Email,
            UserName = userRegistrationDto.UserName,
            FirstName = userRegistrationDto.FirstName,
            LastName = userRegistrationDto.LastName
        };
        var result = await _userManager.CreateAsync(newUser, userRegistrationDto.Password);
        ICollection<string> roles;

        if (result.Succeeded)
        {
            _logger.LogInformation($"User registration successful.");
            if (userRegistrationDto.Roles != null)
            {
                var normalizedRoles = userRegistrationDto.Roles.Select(role => role.ToUpperInvariant()).ToList();
                if (!normalizedRoles.Contains("USER"))
                {
                    normalizedRoles.Add("USER");
                }
                await _userManager.AddToRolesAsync(newUser, normalizedRoles);
                roles = normalizedRoles;
                _logger.LogInformation($"Registering roles to user is successful.");
            }
            else
            {
                await _userManager.AddToRolesAsync(newUser, ["USER"]);
                roles = ["USER"];
            }
        }
        else
        {
            throw new AuthenticationFailureException(result.Errors.ToList()[0].Description);
        }

        var userDetails = new UserDetails
        {
            Id = newUser.Id,
            Email = newUser.Email,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            DateOfBirth = newUser.DateOfBirth,
            Username = newUser.UserName,
            Roles = roles
        };

        return userDetails;
    }

    public async Task<(string, UserDetails)> LoginAsync()
    {
        if (_user == null)
            throw new NotFoundException("User", Guid.Parse(_user!.Id));

        var token = await CreateTokenAsync();
        var roles = await _userManager.GetRolesAsync(_user!);
        var userDetails = new UserDetails
        {
            Id = _user!.Id,
            Email = _user.Email,
            FirstName = _user.FirstName,
            LastName = _user.LastName,
            DateOfBirth = _user.DateOfBirth,
            Username = _user.UserName,
            Roles = roles
        };

        return (token, userDetails);
    }

    private async Task<string> CreateTokenAsync()
    {
        if (_user == null) throw new UnauthorizedAccessException();

        _logger.LogInformation("Registering claims");
        var claims = new List<Claim>
            { new Claim(ClaimTypes.NameIdentifier, _user.Id), new Claim(ClaimTypes.Name, _user.UserName!) };
        var roles = await _userManager.GetRolesAsync(_user);
        if (roles.Any())
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        _logger.LogInformation("Creating signing credentials");
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("DOTNETJWTSECRET") ?? throw new
            InvalidOperationException());
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        _logger.LogInformation("Creating token");
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var jwtToken = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public async Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto)
    {
        _logger.LogInformation("Validating user credentials");
        _user = await _userManager.FindByNameAsync(userAuthenticationDto.Username);

        var result = await _userManager.CheckPasswordAsync(_user!, userAuthenticationDto.Password);

        if (!result)
        {
            _logger.LogWarning("User credentials are invalid");
        }

        return result;
    }

    public async Task ChangeUserInformationAsync(string userId, UserUpdateDto userUpdateDto)
    {
        _logger.LogInformation($"Changing user information of user with id: {userId}");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User", Guid.Parse(userId));

        if (userUpdateDto.FirstName != null) user.FirstName = userUpdateDto.FirstName;
        if (userUpdateDto.LastName != null) user.LastName = userUpdateDto.LastName;
        if (userUpdateDto.Email != null && user.Email != null)
            await _userManager.ChangeEmailAsync(user, user.Email, userUpdateDto.Email);

        if (userUpdateDto.CurrentPassword != null && userUpdateDto.NewPassword != null)
            await _userManager.ChangePasswordAsync(user, userUpdateDto.CurrentPassword, userUpdateDto.NewPassword);

        if (userUpdateDto.Roles != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var normalizedRoles = userUpdateDto.Roles.Select(role => role.ToUpperInvariant()).ToList();
            await _userManager.AddToRolesAsync(user, normalizedRoles);
            _logger.LogInformation($"Registering roles to user is successful.");
        }

        await _userManager.UpdateAsync(user);
    }


    public async Task DeleteUserAsync(string userId)
    {
        _logger.LogInformation($"Deleting user with id: {userId}");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User", Guid.Parse(userId));

        await _userManager.DeleteAsync(user);
    }
}