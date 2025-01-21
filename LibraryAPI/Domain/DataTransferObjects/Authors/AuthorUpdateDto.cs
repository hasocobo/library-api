namespace LibraryAPI.Domain.DataTransferObjects.Authors;

public record AuthorUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    
}