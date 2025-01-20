using LibraryAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Infrastructure;

public class LibraryContext : IdentityDbContext<ApplicationUser>
{
    
}