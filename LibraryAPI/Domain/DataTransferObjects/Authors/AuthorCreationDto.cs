using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Authors;

public record AuthorCreationDto
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string? ApplicationUserId { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
};