using System.ComponentModel.DataAnnotations;
using LibraryAPI.Domain.DataTransferObjects.Books;

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
    
    public ICollection<BookDetailsDto> Books { get; init; } = new List<BookDetailsDto>();
}