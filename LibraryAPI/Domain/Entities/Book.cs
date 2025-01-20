using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }

    [Required] public string BookName { get; set; } = string.Empty;

    public int PageCount { get; set; }

    public int PublishYear { get; set; }

    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }
    
    public Guid GenreId { get; set; }
    public Genre? Genre { get; set; }
    
    public ICollection<BorrowedBook>? BorrowedBooks { get; set; }
}