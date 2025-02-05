using LibraryAPI.Domain.DataTransferObjects.Authors;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IAuthorService
{
    Task<AuthorDetailsDto> GetAuthorByIdAsync(Guid authorId);
    Task<PagedResponse<AuthorDetailsDto>> GetAuthorsAsync(QueryParameters queryParameters);
    
    Task<AuthorDetailsDto> CreateAuthorAsync(AuthorCreationDto author);
    
    Task UpdateAuthorAsync(Guid authorId, AuthorUpdateDto authorUpdateDto);
    
    Task DeleteAuthorAsync(Guid authorId);
}