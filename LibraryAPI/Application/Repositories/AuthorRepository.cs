using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync()
    {
        var query = FindAll();

        var authors = await query.ToListAsync();
        
        return authors;
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid authorId)
    {
        var query = FindByCondition(author => author.Id.Equals(authorId));
        
        var author = await query.FirstOrDefaultAsync();
        
        return author;
    }

    public void CreateAuthor(Author author)
    {
        Create(author);
    }

    public void UpdateAuthor(Author author)
    {
        Update(author);
    }

    public void DeleteAuthor(Author author)
    {
        Delete(author);
    }
}