namespace LibraryAPI.Domain.DataTransferObjects.Users;

public record UserDetails
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Email { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string? Username { get; init; } = string.Empty;
    
}