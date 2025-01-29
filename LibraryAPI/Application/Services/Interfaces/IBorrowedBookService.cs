using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBorrowedBookService
{
    Task<PagedResponse<BorrowedBookDetailsDto>> GetBorrowedBooksAsync(QueryParameters queryParameters);
    Task<BorrowedBookDetailsDto> GetBorrowedBookByIdAsync(Guid borrowedBookId);
    
    Task<PagedResponse<BorrowedBookDetailsDto>> GetBorrowedBooksByUserIdAsync(string userId, QueryParameters queryParameters);
    Task<BorrowedBookDetailsDto?> GetBorrowedBookByUserAndBookId(string userId, Guid bookId);

    Task<BorrowedBookDetailsDto> BorrowABookAsync(BorrowedBookCreationDto borrowedBookCreationDto, string userId);
    Task ReturnABorrowedBookAsync(Guid borrowedBookId, string userId);
    
    Task UpdateABorrowedBookAsync(Guid borrowedBookId, BorrowedBookUpdateDto borrowedBookUpdateDto);
}