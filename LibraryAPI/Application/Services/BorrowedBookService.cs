using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Extensions;

namespace LibraryAPI.Application.Services;

public class BorrowedBookService : IBorrowedBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<BorrowedBookService> _logger;

    public BorrowedBookService(IRepositoryManager repositoryManager, ILogger<BorrowedBookService> logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task<PagedResponse<BorrowedBookDetailsDto>> GetBorrowedBooksAsync(QueryParameters queryParameters)
    {
        var paginatedResponse =
            await _repositoryManager.BorrowedBookRepository.GetBorrowedBooks(queryParameters);
        var borrowedBooks = paginatedResponse.Items as List<BorrowedBook>;
        if (borrowedBooks == null)
        {
            _logger.LogInformation("No borrowed books found");
            return new PagedResponse<BorrowedBookDetailsDto> { Items = Array.Empty<BorrowedBookDetailsDto>() };
        }

        _logger.LogInformation("Retrieving all borrowed books");

        var booksToReturn = borrowedBooks.Select(b => b.ToDetailsDto());

        var newPaginatedResult = new PagedResponse<BorrowedBookDetailsDto>
        {
            Items = booksToReturn,
            PageNumber = paginatedResponse.PageNumber,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages,
            TotalCount = paginatedResponse.TotalCount
        };

        return newPaginatedResult;
    }

    // in case user clicks on an already borrowed book in book catalog
    public async Task<BorrowedBookDetailsDto?> GetBorrowedBookByUserAndBookId(string userId, Guid bookId)
    {
        _logger.LogInformation($"Getting borrowed book by user ID: {userId} and book ID: {bookId}");
        var borrowedBook =
            await _repositoryManager.BorrowedBookRepository.CheckIfTheBookIsBorrowedByUser(userId, bookId);

        if (borrowedBook == null)
            return null;
        //throw new NotFoundException("Borrowed Book", bookId);
        return borrowedBook.ToDetailsDto();
    }

    public async Task<BorrowedBookDetailsDto> GetBorrowedBookByIdAsync(Guid borrowedBookId)
    {
        _logger.LogInformation($"Retrieving borrowed book with ID: {borrowedBookId}");
        var borrowedBook = await _repositoryManager.BorrowedBookRepository.GetBorrowedBookById(borrowedBookId);
        if (borrowedBook == null)
            throw new NotFoundException("Borrowed Book", borrowedBookId);

        return borrowedBook.ToDetailsDto();
    }

    public async Task<PagedResponse<BorrowedBookDetailsDto>> GetBorrowedBooksByUserIdAsync(string userId,
        QueryParameters queryParameters)
    {
        _logger.LogInformation($"Retrieving borrowed books by user with ID: {userId}");
        var paginatedResponse =
            await _repositoryManager.BorrowedBookRepository.GetBorrowedBooksByUserId(userId, queryParameters);
        
        var borrowedBooks = paginatedResponse.Items as List<BorrowedBook>;
        
        if (borrowedBooks == null)
        {
            _logger.LogInformation("No borrowed books found");
            return new PagedResponse<BorrowedBookDetailsDto> { Items = Array.Empty<BorrowedBookDetailsDto>() };
        }
        
        var booksToReturn = borrowedBooks.Select(b => b.ToDetailsDto());

        var newPaginatedResult = new PagedResponse<BorrowedBookDetailsDto>
        {
            Items = booksToReturn,
            PageNumber = paginatedResponse.PageNumber,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages,
            TotalCount = paginatedResponse.TotalCount
        };

        return newPaginatedResult;
    }

    public async Task<BorrowedBookDetailsDto> BorrowABookAsync(BorrowedBookCreationDto borrowedBookCreationDto,
        string userId)
    {
        _logger.LogInformation($"User: {userId}, borrows the book: {borrowedBookCreationDto.BookId}");

        if (!await _repositoryManager.BookRepository.CheckIfBookIsAvailableAsync(borrowedBookCreationDto.BookId))
            throw new NotFoundException("Book", borrowedBookCreationDto.BookId);

        if (await _repositoryManager.BorrowedBookRepository.CheckIfTheBookIsBorrowedByUser(userId,
                borrowedBookCreationDto.BookId) != null)
            throw new ArgumentException($"The book: {borrowedBookCreationDto.BookId} is already borrowed");

        var borrowedBook = new BorrowedBook
        {
            Id = Guid.NewGuid(),
            BookId = borrowedBookCreationDto.BookId,
            BorrowerId = userId,
            BorrowedDate = DateTime.UtcNow,
            DueDate = borrowedBookCreationDto.DueDate
        };
        await using var transaction = await _repositoryManager.BeginTransactionAsync();
        try
        {
            _repositoryManager.BorrowedBookRepository.CreateBorrowedBook(borrowedBook);
            await _repositoryManager.SaveAsync();

            var book = await
                _repositoryManager.BookRepository.GetBookByIdAsync(borrowedBookCreationDto.BookId);

            book!.Quantity = book.Quantity - 1;

            _repositoryManager.BookRepository.UpdateBook(book);
            await _repositoryManager.SaveAsync();

            await transaction.CommitAsync();
        }

        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, "An error occured during borrowing the book");
            throw;
        }

        return borrowedBook.ToDetailsDto();
    }

    public async Task ReturnABorrowedBookAsync(Guid borrowedBookId, string userId)
    {
        _logger.LogInformation($"User: {userId} returns borrowed book with ID: {borrowedBookId}");

        var borrowedBook = await _repositoryManager.BorrowedBookRepository.GetBorrowedBookById(borrowedBookId);
        if (borrowedBook == null)
            throw new NotFoundException("Borrowed Book", borrowedBookId);

        if (borrowedBook.IsReturned == true)
            throw new Exception($"Borrowed book with ID: {borrowedBookId} is already returned");

        await using var transaction = await _repositoryManager.BeginTransactionAsync();
        try
        {
            borrowedBook.ReturnedDate = DateTime.UtcNow;
            borrowedBook.IsReturned = true;

            _repositoryManager.BorrowedBookRepository.UpdateBorrowedBook(borrowedBook);
            await _repositoryManager.SaveAsync();

            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(borrowedBook.BookId);
            book!.Quantity = book.Quantity + 1;
            _repositoryManager.BookRepository.UpdateBook(book);
            await _repositoryManager.SaveAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, "An error occured during returning the book");
            throw;
        }
    }

    public async Task UpdateABorrowedBookAsync(Guid borrowedBookId, BorrowedBookUpdateDto borrowedBookUpdateDto)
    {
        _logger.LogInformation($"Updating borrowed book with ID: {borrowedBookId}");
        var borrowedBook = await _repositoryManager.BorrowedBookRepository.GetBorrowedBookById(borrowedBookId);
        if (borrowedBook == null)
            throw new NotFoundException("Borrowed Book", borrowedBookId);

        if (borrowedBookUpdateDto.DueDate != null) borrowedBook.DueDate = borrowedBookUpdateDto.DueDate;
        if (borrowedBookUpdateDto.IsReturned != null) borrowedBook.IsReturned = (bool)borrowedBookUpdateDto.IsReturned;

        _repositoryManager.BorrowedBookRepository.UpdateBorrowedBook(borrowedBook);
        await _repositoryManager.SaveAsync();
    }
}