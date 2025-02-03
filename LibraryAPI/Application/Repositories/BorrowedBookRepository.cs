using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class BorrowedBookRepository : RepositoryBase<BorrowedBook>, IBorrowedBookRepository
{
    public BorrowedBookRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<bool> CheckIfBorrowedBookExists(Guid borrowedBookId)
    {
        return await FindByCondition(borrowedBook => borrowedBook.Id.Equals(borrowedBookId)).AnyAsync();
    }

    public async Task<BorrowedBook?> CheckIfTheBookIsBorrowedByUser(string userId, Guid bookId)
    {
        var query = FindByCondition(borrowedBook =>
            borrowedBook.BookId.Equals(bookId) && borrowedBook.BorrowerId.Equals(userId) &&
            borrowedBook.IsReturned == false);

        var borrowedBook = await query.Include(b => b.Borrower)
            .Include(bb => bb.Book)
            .ThenInclude(b => b!.Author)
            .Include(b => b.Book)
            .ThenInclude(b => b!.Genre)
            .FirstOrDefaultAsync();

        return borrowedBook;
    }

    public async Task<PagedResponse<BorrowedBook>> GetBorrowedBooks(QueryParameters queryParameters)
    {
        var query = FindByCondition(borrowedBook => borrowedBook.Book!.IsDeleted == false)
            .Include(b => b.Borrower)
            .Include(bb => bb.Book)
            .ThenInclude(b => b!.Author)
            .Include(b => b.Book)
            .ThenInclude(b => b!.Genre) as IQueryable<BorrowedBook>;

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var keywords = queryParameters.SearchTerm
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim().ToLower())
                .ToArray();

            // search query for each keyword. 
            query = query.Where(bb =>
                keywords.Any(keyword =>
                    bb.Borrower != null &&
                    (bb.Borrower.FirstName.ToLower().Contains(keyword) ||
                     bb.Borrower.LastName.ToLower().Contains(keyword))
                    || bb.Book != null && bb.Book.Title.ToLower().Contains(keyword)
                ));
        }

        var totalCount = await query.CountAsync();

        var borrowedBooks = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var pagedResponse = new PagedResponse<BorrowedBook>
        {
            Items = borrowedBooks,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
        };

        return pagedResponse;
    }

    public async Task<BorrowedBook?> GetBorrowedBookById(Guid id)
    {
        var query = FindByCondition(bBook => bBook.Id.Equals(id));

        var borrowedBook = await query
            .Include(b => b.Borrower)
            .Include(bb => bb.Book)
            .ThenInclude(b => b!.Author)
            .Include(b => b.Book)
            .ThenInclude(b => b!.Genre)
            .FirstOrDefaultAsync();

        return borrowedBook;
    }

    public async Task<PagedResponse<BorrowedBook>> GetBorrowedBooksByUserId(string userId,
        QueryParameters queryParameters)
    {
        var query = FindByCondition(bBook => bBook.BorrowerId.Equals(userId) && bBook.Book!.IsDeleted == false)
            .Include(b => b.Borrower)
            .Include(bb => bb.Book)
            .ThenInclude(b => b!.Author)
            .Include(b => b.Book)
            .ThenInclude(b => b!.Genre);


        var totalCount = await query.CountAsync();

        var borrowedBooks = await query
            .OrderBy(b => b.BorrowedDate)
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var pagedResponse = new PagedResponse<BorrowedBook>
        {
            Items = borrowedBooks,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
        };

        return pagedResponse;
    }

    public void CreateBorrowedBook(BorrowedBook borrowedBook)
    {
        Create(borrowedBook);
    }

    public void UpdateBorrowedBook(BorrowedBook borrowedBook)
    {
        Update(borrowedBook);
    }

    public void DeleteBorrowedBook(BorrowedBook borrowedBook)
    {
        Delete(borrowedBook);
    }
}