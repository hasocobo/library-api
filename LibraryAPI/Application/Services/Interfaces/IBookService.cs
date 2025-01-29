using LibraryAPI.Domain.DataTransferObjects;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBookService
{
    Task<BookDetailsDto> CreateBookAsync(BookCreationDto bookCreateDto, Guid authorId, Guid genreId);
    Task<PagedResponse<BookDetailsDto>> GetBooksAsync(QueryParameters queryParameters);
    Task<IEnumerable<BookDetailsDto>> GetDeletedBooksAsync();
    Task<BookDetailsDto> GetDeletedBookByIdAsync(Guid deletedBookId);
    Task<IEnumerable<BookDetailsDto>> GetBooksByAuthorIdAsync(Guid authorId);
    Task<IEnumerable<BookDetailsDto>> GetBooksByGenreIdAsync(Guid genreId);
    
    Task<BookDetailsDto> GetBookByIdAsync(Guid bookId);
    Task UpdateBookAsync(Guid bookId, BookUpdateDto bookUpdateDto);
    Task DeleteBookByIdAsync(Guid bookId);
    Task RestoreDeletedBookByIdAsync(Guid bookId);
    Task RestoreDeletedBooksAsync();
}