using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/")]
public class BorrowedBookController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BorrowedBookController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("users/{userId}/borrowed-books")]
    public async Task<ActionResult<IEnumerable<BorrowedBookDetailsDto>>> GetBorrowedBooksByUserId(string userId)
    {
        var borrowedBooks =
            await _serviceManager.BorrowedBookService.GetBorrowedBooksByUserIdAsync(userId);
        return Ok(borrowedBooks);
    }

    [HttpGet("users/{userId}/borrowed-books/{bookId}")]
    public async Task<ActionResult<BorrowedBookDetailsDto>> GetBorrowedBookByUserAndBookId(string userId, Guid bookId)
    {
        var borrowedBook = 
            await _serviceManager.BorrowedBookService.GetBorrowedBookByUserAndBookId(userId, bookId);
        
        if (borrowedBook == null)
            return NotFound();
        
        return Ok(borrowedBook);
    }
    
    [HttpGet("borrowed-books")]
    public async Task<ActionResult<IEnumerable<BorrowedBookDetailsDto>>> GetBorrowedBooks()
    {
        var borrowedBooks =
            await _serviceManager.BorrowedBookService.GetBorrowedBooksAsync();

        return Ok(borrowedBooks);
    }

    [HttpGet("borrowed-books/{borrowedBookId:guid}")]
    public async Task<ActionResult<BorrowedBookDetailsDto>> GetBorrowedBookById(Guid borrowedBookId)
    {
        var borrowedBook =
            await _serviceManager.BorrowedBookService.GetBorrowedBookByIdAsync(borrowedBookId);

        return Ok(borrowedBook);
    }

    [HttpPost("users/{userId}/borrowed-books")]
    public async Task<ActionResult<BorrowedBookDetailsDto>> BorrowABook(string userId,
        [FromBody] BorrowedBookCreationDto borrowedBookCreationDto)
    {
        var bookToReturn = await _serviceManager.BorrowedBookService.BorrowABookAsync(borrowedBookCreationDto, userId);
        
        return CreatedAtAction(nameof(GetBorrowedBookById), new { borrowedBookId = bookToReturn.Id }, bookToReturn);
    }

    [HttpPut("borrowed-books/{borrowedBookId:guid}")]
    public async Task<ActionResult> UpdateBorrowedBook(Guid borrowedBookId,
        [FromBody] BorrowedBookUpdateDto bookToUpdate)
    {
        await _serviceManager.BorrowedBookService.UpdateABorrowedBookAsync(borrowedBookId, bookToUpdate);
        return Ok();
    }

    [HttpDelete("users/{userId}/borrowed-books/{borrowedBookId:guid}")]
    public async Task<ActionResult> ReturnBorrowedBook(Guid borrowedBookId, string userId)
    {
        await _serviceManager.BorrowedBookService.ReturnABorrowedBookAsync(borrowedBookId, userId);
        return Ok();
    }
}