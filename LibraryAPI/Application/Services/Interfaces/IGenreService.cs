using LibraryAPI.Domain.DataTransferObjects.Genres;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IGenreService
{
    Task<GenreDetailsDto> GetGenreByIdAsync(Guid id);
    Task<PagedResponse<GenreDetailsDto>> GetGenreBySlugAsync(string slug, QueryParameters queryParameters);
    Task<PagedResponse<GenreDetailsDto>> GetAllGenresAsync(QueryParameters queryParameters);
    Task<GenreDetailsDto> CreateGenre(GenreCreationDto genreCreationDto);
    Task UpdateGenreByIdAsync(Guid genreId, GenreUpdateDto genreUpdateDto);
    Task DeleteGenreByIdAsync(Guid id);
}