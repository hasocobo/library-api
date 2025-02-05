using LibraryAPI.Domain.DataTransferObjects.Users;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserDetails> GetUserByIdAsync(string id);
    Task<PagedResponse<UserDetails>> GetUsersAsync(QueryParameters queryParameters);
    Task<UserDetails> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<(string, UserDetails)> LoginAsync();
    Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto);
    
    Task ChangeUserInformationAsync(string userId, UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(string id);

}