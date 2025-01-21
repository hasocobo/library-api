namespace LibraryAPI.Domain.DataTransferObjects.Users;

public record UserAuthenticationDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    
}