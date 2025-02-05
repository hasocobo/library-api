using System.Text.Json;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Genres;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers;

[Route("api/v1/")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public GenresController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<GenreDetailsDto>>> GetAllGenres([FromQuery] QueryParameters queryParameters)
    {
        var pagedResponse = await _serviceManager.GenreService.GetAllGenresAsync(queryParameters);
        var genres = pagedResponse.Items;
        
        var paginationMetadata = new
        {
            PageNumber = pagedResponse.PageNumber,
            TotalPages = pagedResponse.TotalPages,
            PageSize = pagedResponse.PageSize,
            TotalCount = pagedResponse.TotalCount
        };
        
        Response.Headers["LibraryApi-Pagination"] = JsonSerializer.Serialize(paginationMetadata);

        return Ok(genres);
    }

    [HttpGet("genres/{genreId:guid}")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreById(Guid genreId)
    {
        var genre = await _serviceManager.GenreService.GetGenreByIdAsync(genreId);
        return Ok(genre);
    }

    [HttpGet("genres/{slug}")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreBySlug(string slug,
        [FromQuery] QueryParameters queryParameters)
    {
        var pagedResponse = await _serviceManager.GenreService.GetGenreBySlugAsync(slug, queryParameters);
        var paginationMetadata = new
        {
            PageNumber = pagedResponse.PageNumber,
            TotalPages = pagedResponse.TotalPages,
            PageSize = pagedResponse.PageSize,
            TotalCount = pagedResponse.TotalCount
        };
        var genre = pagedResponse.Items.FirstOrDefault();
        Response.Headers["LibraryApi-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        return Ok(genre);
    }

    [HttpPost("genres")]
    public async Task<ActionResult> CreateGenre([FromBody] GenreCreationDto genreCreationDto)
    {
        var genreToReturn = await _serviceManager.GenreService.CreateGenre(genreCreationDto);
        return CreatedAtAction(nameof(GetGenreById), new { genreId = genreToReturn.Id }, genreToReturn);
    }

    [HttpPut("genres/{genreId}")]
    public async Task<ActionResult> UpdateGenre(Guid genreId, [FromBody] GenreUpdateDto genreUpdateDto)
    {
        await _serviceManager.GenreService.UpdateGenreByIdAsync(genreId, genreUpdateDto);
        return Ok();
    }

    [HttpDelete("genres/{genreId}")]
    public async Task<ActionResult> DeleteGenre(Guid genreId)
    {
        await _serviceManager.GenreService.DeleteGenreByIdAsync(genreId);
        return NoContent();
    }
}