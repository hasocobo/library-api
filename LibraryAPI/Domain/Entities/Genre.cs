using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Genre
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public Guid SubGenreId { get; set; }
}