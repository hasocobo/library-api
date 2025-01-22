using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        var query = FindByCondition(book => book.IsDeleted == false);

        var books = await query.ToListAsync();

        return books;
    }

    public async Task<IEnumerable<Book>> GetDeletedBooksAsync()
    {
        var query = FindByCondition(book => book.IsDeleted == true);

        var deletedBooks = await query.ToListAsync();

        return deletedBooks;
    }

    public async Task<Book?> GetBookByIdAsync(Guid bookId)
    {
        var query = FindByCondition(book => book.Id.Equals(bookId) && book.IsDeleted == false);

        var book = await query.FirstOrDefaultAsync();

        return book;
    }

    public async Task<bool> CheckIfBookIsAvailableAsync(Guid bookId)
    {
        var query = FindByCondition(book => book.Id.Equals(bookId) && book.Quantity > 0);
        
        return await query.AnyAsync();
    }

    public async Task<Book?> GetDeletedBookByIdAsync(Guid id)
    {
        var query = FindByCondition(book => book.Id.Equals(id) && book.IsDeleted == true);

        var book = await query.FirstOrDefaultAsync();

        return book;
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid id)
    {
        var query = FindByCondition(book => book.AuthorId.Equals(id) && book.IsDeleted == false);

        var books = await query.ToListAsync();

        return books;
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreIdAsync(Guid id)
    {
        var query = FindByCondition(book => book.GenreId.Equals(id) && book.IsDeleted == false);

        var books = await query.ToListAsync();

        return books;
    }

    public void CreateBook(Book book)
    {
        Create(book);
    }

    public void UpdateBook(Book book)
    {
        Update(book);
    }

    public void DeleteBook(Book book)
    {
        Delete(book);
    }
}