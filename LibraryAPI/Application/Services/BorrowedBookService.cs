using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
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

    public async Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooksAsync()
    {
        var borrowedBooks = await _repositoryManager.BorrowedBookRepository.GetBorrowedBooks() as List<BorrowedBook>;

        if (borrowedBooks == null)
        {
            _logger.LogInformation("No borrowed books found");
            return Array.Empty<BorrowedBookDetailsDto>();
        }

        _logger.LogInformation("Retrieving all borrowed books");

        var borrowedBooksToReturn =
            borrowedBooks.Select(borrowedBook => borrowedBook.ToDetailsDto());

        return borrowedBooksToReturn;
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

    public async Task<IEnumerable<BorrowedBookDetailsDto>> GetBorrowedBooksByUserIdAsync(string userId)
    {
        _logger.LogInformation($"Retrieving borrowed books by user with ID: {userId}");
        var borrowedBooks =
            await _repositoryManager.BorrowedBookRepository.GetBorrowedBooksByUserId(userId);

        return borrowedBooks.Select(borrowedBook => borrowedBook.ToDetailsDto());
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