using LibraryAPI.Domain.DataTransferObjects.Genres;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IGenreService
{
    Task<GenreDetailsDto> GetGenreByIdAsync(Guid id);
    Task<IEnumerable<GenreDetailsDto>> GetAllGenresAsync();
    Task<GenreDetailsDto> CreateGenre(GenreCreationDto genreCreationDto);
    Task UpdateGenreByIdAsync(Guid genreId, GenreUpdateDto genreUpdateDto);
    Task DeleteGenreByIdAsync(Guid id);
}