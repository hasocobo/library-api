using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Authors;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[Route("api/v1/")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthorsController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("authors")]
    public async Task<ActionResult<IEnumerable<AuthorDetailsDto>>> GetAuthors()
    {
        var authors = await _serviceManager.AuthorService.GetAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("authors/{authorId}")]
    public async Task<ActionResult<AuthorDetailsDto>> GetAuthorById(Guid authorId)
    {
        var author = await _serviceManager.AuthorService.GetAuthorByIdAsync(authorId);
        return Ok(author);
    }

    [HttpPost("authors")]
    public async Task<ActionResult<AuthorDetailsDto>> CreateAuthor(AuthorCreationDto authorCreationDto)
    {
        var authorToReturn = await _serviceManager.AuthorService.CreateAuthorAsync(authorCreationDto);
        return CreatedAtAction(nameof(GetAuthorById), new { authorId = authorToReturn.Id }, authorToReturn);
    }

    [HttpDelete("authors/{authorId}")]
    public async Task<ActionResult> DeleteAuthorById(Guid authorId)
    {
        await _serviceManager.AuthorService.DeleteAuthorAsync(authorId);
        return NoContent();
    }

    [HttpPut("authors/{authorId}")]
    public async Task<ActionResult> UpdateAuthorById(Guid authorId, AuthorUpdateDto authorUpdateDto)
    {
        await _serviceManager.AuthorService.UpdateAuthorAsync(authorId, authorUpdateDto);
        return Ok();
    }
}