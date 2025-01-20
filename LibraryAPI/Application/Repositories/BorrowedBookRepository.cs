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

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooks()
    {
        var query = FindAll();
        
        var borrowedBooks = await query.ToListAsync();

        return borrowedBooks;
    }

    public async Task<BorrowedBook?> GetBorrowedBookById(Guid id)
    {
        var query = FindByCondition(bBook => bBook.BorrowedBookId.Equals(id));
        
        var borrowedBook = await query.FirstOrDefaultAsync();
        
        return borrowedBook;
    }

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooksByUserId(Guid userId)
    {
        var query = FindByCondition(bBook => bBook.BorrowedBookId.Equals(userId));
        
        var borrowedBooks = await query.ToListAsync();
        
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