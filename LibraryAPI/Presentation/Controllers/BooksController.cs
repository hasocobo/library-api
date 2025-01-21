using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.Entities;
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

    [HttpGet("books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooks()
    {
        var books = await _serviceManager.BookService.GetBooksAsync();

        return Ok(books);
    }

    [HttpGet("books/{bookId:guid}")]
    public async Task<ActionResult<BookDetailsDto>> GetBookById(Guid bookId)
    {
        var book = await _serviceManager.BookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }

    [HttpGet("authors/{authorId:guid}/books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooksByAuthorId(Guid authorId)
    {
        var books = await _serviceManager.BookService.GetBooksByAuthorIdAsync(authorId);
        return Ok(books);
    }
    
    [HttpGet("genres/{genreId:guid}/books")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooksByGenreId(Guid genreId)
    {
        var books = await _serviceManager.BookService.GetBooksByGenreIdAsync(genreId);
        return Ok(books);
    }
    

    [HttpPost("authors/{authorId:guid}/books")]
    public async Task<ActionResult<BookDetailsDto>> CreateBook([FromBody] BookCreationDto bookToCreate, Guid authorId,
        Guid genreId)
    {
        var bookToReturn = await _serviceManager.BookService.CreateBookAsync(bookToCreate, authorId, genreId);
        return CreatedAtAction(nameof(GetBookById), new { bookId = bookToReturn.Id }, bookToReturn);
    }

    [HttpPut("books/{bookId:guid}")]
    public async Task<ActionResult> UpdateBook(Guid bookId, [FromBody] BookUpdateDto bookToUpdate)
    {
        await _serviceManager.BookService.UpdateBookAsync(bookId, bookToUpdate);
        return Ok();
    }
    
    [HttpDelete("books/{bookId:guid}")]
    public async Task<ActionResult> DeleteBook(Guid bookId)
    {
        await _serviceManager.BookService.DeleteBookByIdAsync(bookId);
        return NoContent();
    }
}