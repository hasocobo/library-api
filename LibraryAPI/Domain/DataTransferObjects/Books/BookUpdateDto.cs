using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Books;

public record BookUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string? Title { get; init; }
    
    public Guid? AuthorId { get; init; }
    
    public Guid? GenreId { get; init; }
    
    public int? PageCount { get; init; }
}