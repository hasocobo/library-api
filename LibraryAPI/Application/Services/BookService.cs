using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Extensions;

namespace LibraryAPI.Application.Services;

public class BookService : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<BookService> _logger;

    public BookService(IRepositoryManager repositoryManager, ILogger<BookService> logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task<BookDetailsDto> CreateBookAsync(BookCreationDto bookCreateDto, Guid authorId, Guid genreId)
    {
        //TODO: author ve genre null olup olmadığını kontrol et
        _logger.LogInformation($"Creating new book for genre {genreId} and author {authorId}. ");
        var book = new Book
        {
            Title = bookCreateDto.Title,
            AuthorId = authorId,
            GenreId = genreId,
            PageCount = bookCreateDto.PageCount,
            PublishYear = bookCreateDto.PublishYear,
            Quantity = bookCreateDto.Quantity,
            ImageUrl = bookCreateDto.ImageUrl,
        };

        _repositoryManager.BookRepository.CreateBook(book);
        await _repositoryManager.SaveAsync();

        _logger.LogInformation("Returning book details");
        var bookToReturn = book.ToDetailsDto();

        return bookToReturn;
    }

    public async Task<PagedResponse<BookDetailsDto>> GetBooksAsync(QueryParameters queryParameters)
    {
        _logger.LogInformation(
            $"Retrieving books at page {queryParameters.PageNumber} with page size {queryParameters.PageSize}.");
        var paginatedResponse = (await _repositoryManager.BookRepository.GetBooksAsync(queryParameters));

        var books = paginatedResponse.Items as List<Book>;
        if (books == null)
        {
            _logger.LogInformation("No books found");
            return new PagedResponse<BookDetailsDto>
            {
                Items = Array.Empty<BookDetailsDto>()
            };
        }

        _logger.LogInformation("Returning book details");
        var booksToReturn = books.Select(b => b.ToDetailsDto());

        var newPaginatedResult = new PagedResponse<BookDetailsDto>
        {
            Items = booksToReturn,
            PageNumber = paginatedResponse.PageNumber,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages,
            TotalCount = paginatedResponse.TotalCount
        };
        
        return newPaginatedResult;
    }

    public async Task<IEnumerable<BookDetailsDto>> GetBooksByAuthorIdAsync(Guid authorId)
    {
        _logger.LogInformation($"Retrieving books for author with ID: {authorId}");
        var books = (await _repositoryManager.BookRepository.GetBooksByAuthorIdAsync(authorId)) as List<Book>;

        if (books == null)
        {
            _logger.LogInformation("No books found");
            return Array.Empty<BookDetailsDto>();
        }

        _logger.LogInformation("Returning book details");
        var booksToReturn = books.Select(b => b.ToDetailsDto());
        return booksToReturn;
    }

    public async Task<IEnumerable<BookDetailsDto>> GetBooksByGenreIdAsync(Guid genreId)
    {
        _logger.LogInformation($"Retrieving books by genre with ID: {genreId}");
        var books = (await _repositoryManager.BookRepository.GetBooksByGenreIdAsync(genreId)) as List<Book>;

        if (books == null)
        {
            _logger.LogInformation("No books found");
            return Array.Empty<BookDetailsDto>();
        }

        _logger.LogInformation("Returning book details");
        var booksToReturn = books.Select(b => b.ToDetailsDto());
        return booksToReturn;
    }

    public async Task<IEnumerable<BookDetailsDto>> GetDeletedBooksAsync()
    {
        _logger.LogInformation("Retrieving deleted books");
        var books = await _repositoryManager.BookRepository.GetDeletedBooksAsync();

        return books.Select(b => b.ToDetailsDto());
    }

    public async Task<BookDetailsDto> GetDeletedBookByIdAsync(Guid bookId)
    {
        _logger.LogInformation($"Retrieving deleted book with ID: {bookId}");
        var book = await _repositoryManager.BookRepository.GetDeletedBookByIdAsync(bookId);

        if (book == null)
            throw new NotFoundException("Deleted book", bookId);

        return book.ToDetailsDto();
    }

    public async Task<BookDetailsDto> GetBookByIdAsync(Guid bookId)
    {
        _logger.LogInformation($"Retrieving book with ID: {bookId}");
        var book = await _repositoryManager.BookRepository.GetBookByIdAsync(bookId);
        if (book == null)
            throw new NotFoundException("Book", bookId);

        _logger.LogInformation("Returning book details");
        var bookToReturn = book.ToDetailsDto();
        return bookToReturn;
    }

    public async Task UpdateBookAsync(Guid bookId, BookUpdateDto bookUpdateDto)
    {
        _logger.LogInformation($"Updating book with ID: {bookId}");
        var bookToUpdate = await _repositoryManager.BookRepository.GetBookByIdAsync(bookId);

        if (bookToUpdate == null)
            throw new NotFoundException("Book", bookId);

        if (bookUpdateDto.Title != null)
            bookToUpdate.Title = bookUpdateDto.Title;

        if (bookUpdateDto.AuthorId != null)
            bookToUpdate.AuthorId = (Guid)bookUpdateDto.AuthorId;

        if (bookUpdateDto.GenreId != null)
            bookToUpdate.GenreId = (Guid)bookUpdateDto.GenreId;

        if (bookUpdateDto.PageCount != null)
            bookToUpdate.PageCount = (int)bookUpdateDto.PageCount;

        if (bookUpdateDto.Quantity != null)
            bookToUpdate.Quantity = (int)bookUpdateDto.Quantity;

        if (bookUpdateDto.ImageUrl != null)
            bookToUpdate.ImageUrl = bookUpdateDto.ImageUrl;

        _repositoryManager.BookRepository.UpdateBook(bookToUpdate);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteBookByIdAsync(Guid bookId)
    {
        _logger.LogInformation($"Deleting book with ID: {bookId}");
        var bookToDelete = await _repositoryManager.BookRepository.GetBookByIdAsync(bookId);
        if (bookToDelete == null)
            throw new NotFoundException("Book", bookId);

        bookToDelete.IsDeleted = true;
        _repositoryManager.BookRepository.UpdateBook(bookToDelete);
        await _repositoryManager.SaveAsync();
    }

    public async Task RestoreDeletedBookByIdAsync(Guid bookId)
    {
        _logger.LogInformation($"Restoring deleted book with ID: {bookId}");
        var bookToRestore = await _repositoryManager.BookRepository.GetDeletedBookByIdAsync(bookId);
        if (bookToRestore == null)
            throw new NotFoundException("Book", bookId);

        bookToRestore.IsDeleted = false;
        _repositoryManager.BookRepository.UpdateBook(bookToRestore);
        await _repositoryManager.SaveAsync();
    }

    public async Task RestoreDeletedBooksAsync()
    {
        _logger.LogInformation("Restoring deleted books");
        var books = await _repositoryManager.BookRepository.GetDeletedBooksAsync() as List<Book>;

        if (books == null)
            return;

        foreach (var book in books)
        {
            book.IsDeleted = false;
        }

        await _repositoryManager.SaveAsync();
    }
}