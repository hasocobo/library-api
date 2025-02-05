using System.Text.Json;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Authors;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[Authorize(Policy = "Librarian")]
[Route("api/v1/")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthorsController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [AllowAnonymous]
    [HttpGet("authors")]
    public async Task<ActionResult<IEnumerable<AuthorDetailsDto>>> GetAuthors([FromQuery] QueryParameters queryParameters)
    {
        var pagedResponse = await _serviceManager.AuthorService.GetAuthorsAsync(queryParameters);
        
        var paginationMetadata = new
        {
            PageNumber = pagedResponse.PageNumber,
            TotalPages = pagedResponse.TotalPages,
            PageSize = pagedResponse.PageSize,
            TotalCount = pagedResponse.TotalCount
        };
        Response.Headers["LibraryApi-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        var authors = pagedResponse.Items;
        return Ok(authors);
    }

    [AllowAnonymous]
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