using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;    
    
    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public string LastName { get; set; } = string.Empty;
    
    public DateTime? DateOfBirth { get; set; }
    
}