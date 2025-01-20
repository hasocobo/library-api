using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class GenreRepository :  RepositoryBase<Genre>, IGenreRepository
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