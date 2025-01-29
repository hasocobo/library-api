using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetGenresAsync();
    Task<Genre?> GetGenreByIdAsync(Guid id);
    Task<PagedResponse<Genre>> GetGenreBySlugAsync(string slug, QueryParameters queryParameters);
    
    void CreateGenre(Genre genre);
    void UpdateGenre(Genre genre);
    void DeleteGenre(Genre genre);
}