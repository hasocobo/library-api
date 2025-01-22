using LibraryAPI.Domain.DataTransferObjects.Users;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserDetails> GetUserByIdAsync(string id);
    Task<IEnumerable<UserDetails>> GetUsersAsync();
    Task<UserDetails> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<string> CreateTokenAsync();
    Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto);
    
    Task ChangeUserInformationAsync(string userId, UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(string id);

}