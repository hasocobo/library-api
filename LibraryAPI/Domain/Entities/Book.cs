using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    
    [Required]
    public string BookName { get; set; } = string.Empty;
    
    public int PageCount { get; set; } 
    
    public int PublishYear { get; set; }
}