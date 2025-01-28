using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IBorrowedBookRepository
{
    Task<bool> CheckIfBorrowedBookExists(Guid id);
    Task<BorrowedBook?> CheckIfTheBookIsBorrowedByUser(string userId, Guid bookId);
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooks();
    Task<BorrowedBook?> GetBorrowedBookById(Guid id);
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooksByUserId(string userId);
    
    void CreateBorrowedBook(BorrowedBook borrowedBook);
    
    void UpdateBorrowedBook(BorrowedBook borrowedBook);
    void DeleteBorrowedBook(BorrowedBook borrowedBook);
}