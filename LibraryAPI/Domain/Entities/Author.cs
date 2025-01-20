namespace LibraryAPI.Domain.Entities;

public class Author
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    
    public DateTime? DateOfDeath { get; set; }
    
    public Guid? ApplicationUserId { get; set; }
    public ApplicationUser? User { get; set; }
}