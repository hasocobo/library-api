using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Genres;

public record GenreUpdateDto
{
    public Guid? ParentGenreId { get; init; }
    [MaxLength(50)]

    public string? Name { get; init; }
    
    [MaxLength(50)]
    public string? Slug { get; init; } = string.Empty;
}