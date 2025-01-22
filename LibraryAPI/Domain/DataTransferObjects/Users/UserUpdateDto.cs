namespace LibraryAPI.Domain.DataTransferObjects.Users;

public record UserUpdateDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    
    public string? CurrentPassword { get; init; }
    public string? NewPassword { get; init; }
    public ICollection<string>? Roles { get; init; } 
}