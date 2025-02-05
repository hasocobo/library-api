using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<PagedResponse<Book>> GetBooksAsync(QueryParameters queryParameters)
    {
        var query = FindByCondition(book =>
                book.IsDeleted == false &&
                (queryParameters.GenreId == null || book.GenreId == queryParameters.GenreId) &&
                (queryParameters.AuthorId == null || book.AuthorId == queryParameters.AuthorId))
            .Include(book => book.Author)
            .Include(book => book.Genre) as IQueryable<Book>;

        if (queryParameters.SortDescending != null)
        {
            query = (bool)queryParameters.SortDescending 
                ? query.OrderByDescending(book => book.Title )
                : query.OrderBy(book => book.Title);
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var keywords = queryParameters.SearchTerm
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim().ToLower())
                .ToArray();

            // search query for each keyword. 
            query = query.Where(b =>
                keywords.Any(keyword =>
                    b.Title.ToLower().Contains(keyword) || //tolower yerine StringOptions.OrdinalIgnoreCase ekle 
                    (b.Description != null &&
                     b.Description.ToLower().Contains(keyword) ||
                     b.Author!.FirstName.ToLower().Contains(keyword) ||
                     b.Author.LastName.ToLower().Contains(keyword))
                ));
        }

        var totalCount = await query.CountAsync();

        var books = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var pagedResponse = new PagedResponse<Book>
        {
            Items = books,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
        };

        return pagedResponse;
    }

    public async Task<IEnumerable<Book>> GetDeletedBooksAsync()
    {
        var query = FindByCondition(book => book.IsDeleted == true);

        var deletedBooks = await query.ToListAsync();

        return deletedBooks;
    }

    public async Task<Book?> GetBookByIdAsync(Guid bookId)
    {
        var query = FindByCondition(book => book.Id.Equals(bookId) && book.IsDeleted == false)
            .Include(book => book.Author)
            .Include(book => book.Genre);

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