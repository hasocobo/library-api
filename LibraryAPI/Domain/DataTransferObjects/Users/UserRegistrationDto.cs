using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Users;

public record UserRegistrationDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public ICollection<string>? Roles { get; init; } = new List<string>(){"USER"};
}