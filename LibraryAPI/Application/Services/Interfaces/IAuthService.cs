using LibraryAPI.Domain.DataTransferObjects.Users;
using LibraryAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserDetails> GetUserByIdAsync(string id);
    Task<IEnumerable<UserDetails>> GetUsersAsync();
    Task<UserDetails> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<(string, UserDetails)> LoginAsync();
    Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto);
    
    Task ChangeUserInformationAsync(string userId, UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(string id);

}