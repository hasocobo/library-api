using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Genres;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Extensions;

namespace LibraryAPI.Application.Services;

public class GenreService : IGenreService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IRepositoryManager repositoryManager, ILogger<GenreService> logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task<GenreDetailsDto> GetGenreByIdAsync(Guid genreId)
    {
        _logger.LogInformation($"Retrieving genre with ID: {genreId}");
        var genre = await _repositoryManager.GenreRepository.GetGenreByIdAsync(genreId);
        if (genre == null)
            throw new NotFoundException("Genre", genreId);
        
        _logger.LogInformation($"Returning genre details");
        return genre.ToDetailsDto();  
    }

    public async Task<IEnumerable<GenreDetailsDto>> GetAllGenresAsync()
    {
        _logger.LogInformation($"Retrieving all genres");
        var genres = await _repositoryManager.GenreRepository.GetGenresAsync() as List<Genre>;
        if (genres == null)
        {
            _logger.LogInformation("No genres found");
            return Array.Empty<GenreDetailsDto>();
        }
        
        _logger.LogInformation($"Returning {genres.Count} genres with details");
        return genres.Select(g => g.ToDetailsDto());
    }

    public async Task<GenreDetailsDto> CreateGenre(GenreCreationDto genreCreationDto)
    {
        _logger.LogInformation($"Creating new genre");
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = genreCreationDto.Name,
            ParentGenreId = genreCreationDto.ParentGenreId
        };
        
        _repositoryManager.GenreRepository.CreateGenre(genre);
        await _repositoryManager.SaveAsync();
        
        _logger.LogInformation($"Returning created genre details");
        return genre.ToDetailsDto();
    }

    public async Task UpdateGenreByIdAsync(Guid genreId, GenreUpdateDto genreUpdateDto)
    {
        _logger.LogInformation($"Updating genre with ID: {genreId}");
        var genreToUpdate = await _repositoryManager.GenreRepository.GetGenreByIdAsync(genreId);
        
        if (genreToUpdate == null)
            throw new NotFoundException("Genre", genreId);
        
        if (genreUpdateDto.Name != null) genreToUpdate.Name = genreUpdateDto.Name;
        if (genreUpdateDto.ParentGenreId != null) genreToUpdate.ParentGenreId = genreUpdateDto.ParentGenreId;
        
        _repositoryManager.GenreRepository.UpdateGenre(genreToUpdate);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteGenreByIdAsync(Guid id)
    {
        _logger.LogInformation($"Deleting genre with ID: {id}");
        var genreToDelete = await _repositoryManager.GenreRepository.GetGenreByIdAsync(id);
        
        if (genreToDelete == null)
            throw new NotFoundException("Genre", id);
        
        _repositoryManager.GenreRepository.DeleteGenre(genreToDelete);
        await _repositoryManager.SaveAsync();
    }
}