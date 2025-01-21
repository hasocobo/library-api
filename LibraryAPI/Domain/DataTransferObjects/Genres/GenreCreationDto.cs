using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Genres;

public record GenreCreationDto
{
    [Required]
    [MaxLength(20)]
    public required string Name { get; init; } 
    
    public Guid? ParentGenreId { get; init; }
}