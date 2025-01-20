using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IBorrowedBookRepository
{
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooks();
    Task<BorrowedBook> GetBorrowedBookById(Guid id);
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooksByUserId(Guid userId);
    
    void CreateBorrowedBook(BorrowedBook borrowedBook);
    void UpdateBorrowedBook(BorrowedBook borrowedBook);
    void DeleteBorrowedBook(BorrowedBook borrowedBook);
}