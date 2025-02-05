using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<PagedResponse<Author>> GetAuthorsAsync(QueryParameters queryParameters);
    Task<Author?> GetAuthorByIdAsync(Guid authorId);
    
    void CreateAuthor(Author author);
    void UpdateAuthor(Author author);
    void DeleteAuthor(Author author);
}