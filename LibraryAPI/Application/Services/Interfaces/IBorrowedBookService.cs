using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBorrowedBookService
{
    Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooks();
    Task<BorrowedBookDetailsDto> GetBorrowedBookById(Guid borrowedBookId);
    
    Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooksByUserId(string userId);

    Task<BorrowedBookDetailsDto> BorrowABook(BorrowedBookCreationDto borrowedBookCreationDto, string userId);
    Task ReturnBorrowedBook(Guid borrowedBookId, string userId);
    
    Task UpdateBorrowedBook(Guid borrowedBookId, BorrowedBookUpdateDto borrowedBookUpdateDto);
}