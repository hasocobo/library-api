using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Genres;

public record GenreCreationDto
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; init; } 
    
    [Required]
    [MaxLength(50)]
    public required string Slug { get; init; }
    
    public Guid? ParentGenreId { get; init; }
}