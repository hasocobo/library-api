using System.Text.Json;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[Route("api/v1/")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BooksController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    
    [AllowAnonymous]
    [HttpGet("books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooks([FromQuery] QueryParameters queryParameters)
    {
        var pagedResponse = await _serviceManager.BookService.GetBooksAsync(queryParameters);
        
        var paginationMetadata = new
        {
            PageNumber = pagedResponse.PageNumber,
            TotalPages = pagedResponse.TotalPages,
            PageSize = pagedResponse.PageSize,
            TotalCount = pagedResponse.TotalCount
        };
        Response.Headers["LibraryApi-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        var books = pagedResponse.Items;
        
        return Ok(books);
    }

    [Authorize(Policy = "Librarian")]
    [HttpGet("deleted-books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetDeletedBooks()
    {
        var books = await _serviceManager.BookService.GetDeletedBooksAsync();
        return Ok(books);
    }

    [Authorize(Policy = "Librarian")]
    [HttpGet("deleted-books/{deletedBookId}")]
    public async Task<ActionResult<BookDetailsDto>> GetDeletedBookById(Guid deletedBookId)
    {
        var deletedBook = await _serviceManager.BookService.GetDeletedBookByIdAsync(deletedBookId);
        return Ok(deletedBook);
    }
    
    [Authorize(Policy = "Librarian")]
    [HttpDelete("deleted-books/{deletedBookId}")]
    public async Task<ActionResult<BookDetailsDto>> RestoreDeletedBookById(Guid deletedBookId)
    {
        await _serviceManager.BookService.RestoreDeletedBookByIdAsync(deletedBookId);
        return NoContent();
    }

    [Authorize(Policy = "Librarian")]
    [HttpDelete("deleted-books")]
    public async Task<ActionResult<BookDetailsDto>> RestoreDeletedBooks()
    {
        await _serviceManager.BookService.RestoreDeletedBooksAsync();
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("books/{bookId:guid}")]
    public async Task<ActionResult<BookDetailsDto>> GetBookById(Guid bookId)
    {
        var book = await _serviceManager.BookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }

    [AllowAnonymous]
    [HttpGet("authors/{authorId:guid}/books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooksByAuthorId(Guid authorId)
    {
        var books = await _serviceManager.BookService.GetBooksByAuthorIdAsync(authorId);
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet("genres/{genreId:guid}/books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooksByGenreId(Guid genreId)
    {
        var books = await _serviceManager.BookService.GetBooksByGenreIdAsync(genreId);
        return Ok(books);
    }

    [Authorize(Policy = "Librarian")]
    [HttpPost("authors/{authorId:guid}/books")]
    public async Task<ActionResult<BookDetailsDto>> CreateBook([FromBody] BookCreationDto bookToCreate, Guid authorId,
        Guid genreId)
    {
        var bookToReturn = await _serviceManager.BookService.CreateBookAsync(bookToCreate, authorId, genreId);
        return CreatedAtAction(nameof(GetBookById), new { bookId = bookToReturn.BookId }, bookToReturn);
    }

    [Authorize(Policy = "Librarian")]
    [HttpPut("books/{bookId:guid}")]
    public async Task<ActionResult> UpdateBook(Guid bookId, [FromBody] BookUpdateDto bookToUpdate)
    {
        await _serviceManager.BookService.UpdateBookAsync(bookId, bookToUpdate);
        return Ok();
    }

    [Authorize(Policy = "Librarian")]
    [HttpDelete("books/{bookId:guid}")]
    public async Task<ActionResult> DeleteBook(Guid bookId)
    {
        await _serviceManager.BookService.DeleteBookByIdAsync(bookId);
        return NoContent();
    }
}