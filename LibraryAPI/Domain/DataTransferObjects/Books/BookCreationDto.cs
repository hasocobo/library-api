using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Books;

public record BookCreationDto
{
    [Required]
    [MaxLength(50)]
    public required string Title { get; set; }
    
    public int PublishYear { get; set; }
    
    public int PageCount { get; set; }

    public int Quantity { get; set; } = 1;

}