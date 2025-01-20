using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Author
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    
    public DateTime? DateOfDeath { get; set; }
    [MaxLength(128)]
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? User { get; set; }
    
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}