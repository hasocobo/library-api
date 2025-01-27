using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Genres;

public record GenreDetailsDto
{
    public Guid Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; init; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Slug { get; init; } = string.Empty;
    
    public Guid? ParentGenreId { get; init; }
}