using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
{
    public GenreRepository(LibraryContext libraryContext) : base(libraryContext)
    {
    }

    public async Task<IEnumerable<Genre>> GetGenresAsync()
    {
        var query = FindAll();

        var genres = await query.ToListAsync();

        return genres;
    }

    public async Task<Genre?> GetGenreByIdAsync(Guid id)
    {
        var query = FindByCondition(genre => genre.Id.Equals(id));

        var genre = await query.FirstOrDefaultAsync();

        return genre;
    }

    public async Task<PagedResponse<Genre>> GetGenreBySlugAsync(string slug, QueryParameters queryParameters)
    {
        var query = FindByCondition(genre => genre.Slug.Equals(slug))
            .Include(genre => genre.Books
                /*.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize))  // sqlite desteklemiyor
        */).ThenInclude(b => b.Author);

        var totalCount = await query.CountAsync();

        var genre = await query.ToListAsync();

        var pagedResponse = new PagedResponse<Genre>
        {
            Items = genre,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize),
        };

        return pagedResponse;
    }

    public void CreateGenre(Genre genre)
    {
        Create(genre);
    }

    public void UpdateGenre(Genre genre)
    {
        Update(genre);
    }

    public void DeleteGenre(Genre genre)
    {
        Delete(genre);
    }
}