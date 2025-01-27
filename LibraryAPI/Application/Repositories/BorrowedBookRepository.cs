using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
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

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooks()
    {
        var query = FindAll();

        var borrowedBooks = await query
            .Include(b => b.Borrower)
            .Include(bb => bb.Book)
            .ThenInclude(b => b!.Author)
            .Include(b => b.Book)
            .ThenInclude(b => b!.Genre)
            .ToListAsync();

        return borrowedBooks;
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

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooksByUserId(string userId)
    {
        var query = FindByCondition(bBook => bBook.BorrowerId.Equals(userId));

        var borrowedBooks = await query
                .Include(b => b.Borrower)
                .Include(bb => bb.Book)
                .ThenInclude(b => b!.Author)
                .Include(b => b.Book)
                .ThenInclude(b => b!.Genre)
            .ToListAsync();

        return borrowedBooks;
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