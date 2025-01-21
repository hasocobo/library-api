using LibraryAPI.Domain.DataTransferObjects.Authors;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IAuthorService
{
    Task<AuthorDetailsDto> GetAuthorByIdAsync(Guid authorId);
    Task<IEnumerable<AuthorDetailsDto>> GetAuthorsAsync();
    
    Task<AuthorDetailsDto> CreateAuthorAsync(AuthorCreationDto author);
    
    Task UpdateAuthorAsync(Guid authorId, AuthorUpdateDto authorUpdateDto);
    
    Task DeleteAuthorAsync(Guid authorId);
}