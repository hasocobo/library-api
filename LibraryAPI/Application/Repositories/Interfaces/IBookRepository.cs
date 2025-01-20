using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid id);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(Guid id);
    
    void CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
    
}