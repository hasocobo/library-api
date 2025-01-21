using LibraryAPI.Domain.DataTransferObjects;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBookService
{
    Task<BookDetailsDto> CreateBookAsync(BookCreationDto bookCreateDto, Guid authorId, Guid genreId);
    Task<IEnumerable<BookDetailsDto>> GetAllBooksAsync();
    Task<BookDetailsDto> GetBookByIdAsync(Guid bookId);
    Task UpdateBookAsync(Guid bookId, BookUpdateDto bookUpdateDto);
    Task DeleteBookByIdAsync(Guid bookId);
}