using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<PagedResponse<Author>> GetAuthorsAsync(QueryParameters queryParameters)
    {
        var query = FindAll();

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var keywords = queryParameters.SearchTerm
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim().ToLower())
                .ToArray();

            // search query for each keyword. 
            query = query.Where(author => keywords.Any(keyword
                => author.FirstName.ToLower().Contains(keyword) ||
                   author.LastName.ToLower().Contains(keyword) ||
                   (author.Bio != null && author.Bio.ToLower().Contains(keyword)))
            );
        }

        var totalCount = await query.CountAsync();


        var authors = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize).ToListAsync();
        
        var pagedResponse = new PagedResponse<Author>
        {
            Items = authors,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
        };

        return pagedResponse;
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