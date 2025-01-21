namespace LibraryAPI.Domain.DataTransferObjects.Authors;

public record AuthorDetailsDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    
    public DateTime? DateOfBirth { get; init; }
    public DateTime? DateOfDeath { get; init; }
    public string? ApplicationUserId { get; init; }
};