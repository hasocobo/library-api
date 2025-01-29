using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IBorrowedBookRepository
{
    Task<bool> CheckIfBorrowedBookExists(Guid id);
    Task<BorrowedBook?> CheckIfTheBookIsBorrowedByUser(string userId, Guid bookId);
    Task<PagedResponse<BorrowedBook>> GetBorrowedBooks(QueryParameters queryParameters);
    Task<BorrowedBook?> GetBorrowedBookById(Guid id);
    Task<PagedResponse<BorrowedBook>> GetBorrowedBooksByUserId(string userId, QueryParameters queryParameters);
    
    void CreateBorrowedBook(BorrowedBook borrowedBook);
    
    void UpdateBorrowedBook(BorrowedBook borrowedBook);
    void DeleteBorrowedBook(BorrowedBook borrowedBook);
}