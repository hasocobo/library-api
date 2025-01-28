using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBorrowedBookService
{
    Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooksAsync();
    Task<BorrowedBookDetailsDto> GetBorrowedBookByIdAsync(Guid borrowedBookId);
    
    Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooksByUserIdAsync(string userId);
    Task<BorrowedBookDetailsDto?> GetBorrowedBookByUserAndBookId(string userId, Guid bookId);

    Task<BorrowedBookDetailsDto> BorrowABookAsync(BorrowedBookCreationDto borrowedBookCreationDto, string userId);
    Task ReturnABorrowedBookAsync(Guid borrowedBookId, string userId);
    
    Task UpdateABorrowedBookAsync(Guid borrowedBookId, BorrowedBookUpdateDto borrowedBookUpdateDto);
}