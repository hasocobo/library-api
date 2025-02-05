using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<PagedResponse<Genre>> GetGenresAsync(QueryParameters queryParameters);
    Task<Genre?> GetGenreByIdAsync(Guid id);
    Task<PagedResponse<Genre>> GetGenreBySlugAsync(string slug, QueryParameters queryParameters);
    
    void CreateGenre(Genre genre);
    void UpdateGenre(Genre genre);
    void DeleteGenre(Genre genre);
}