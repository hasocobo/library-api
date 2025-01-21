using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Genre
{
    public Guid Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public Guid? ParentGenreId { get; set; }
    public Genre? ParentGenre { get; set; }
    
    public ICollection<Genre> SubGenres { get; set; } = new HashSet<Genre>(); 
    
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}