using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Genres;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Domain.QueryFeatures;
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

    public async Task<PagedResponse<GenreDetailsDto>> GetGenreBySlugAsync(string slug, QueryParameters queryParameters)
    {
        _logger.LogInformation($"Retrieving genre with slug name: {slug}");
        var pagedResponse = await _repositoryManager.GenreRepository.GetGenreBySlugAsync(slug, queryParameters);
        var genreToReturn = pagedResponse.Items.Select(g => g.ToDetailsDto()).ToList();

        if (genreToReturn.FirstOrDefault() == null)
            throw new Exception($"Genre with slug name {slug} not found.");

        _logger.LogInformation($"Returning genre details");

        var newPaginatedResult = new PagedResponse<GenreDetailsDto>
        {
            Items = genreToReturn,
            PageNumber = pagedResponse.PageNumber,
            PageSize = pagedResponse.PageSize,
            TotalPages = pagedResponse.TotalPages,
            TotalCount = pagedResponse.TotalCount
        };
        return newPaginatedResult;
    }

    public async Task<PagedResponse<GenreDetailsDto>> GetAllGenresAsync(QueryParameters queryParameters)
    {
        _logger.LogInformation(
            $"Retrieving genres at page {queryParameters.PageNumber} with page size {queryParameters.PageSize}.");
        var pagedResponse = await _repositoryManager.GenreRepository.GetGenresAsync(queryParameters);

        var genres = pagedResponse.Items as List<Genre>;
        if (genres == null)
        {
            _logger.LogInformation("No genres found");
            return new PagedResponse<GenreDetailsDto>()
            {
                Items = Array.Empty<GenreDetailsDto>()
            };
        }

        _logger.LogInformation($"Returning {genres.Count} genres with details");
        var genresToReturn = genres.Select(g => g.ToDetailsDto());

        var newPagedResponse = new PagedResponse<GenreDetailsDto>()
        {
            Items = genresToReturn,
            PageNumber = pagedResponse.PageNumber,
            PageSize = pagedResponse.PageSize,
            TotalPages = pagedResponse.TotalPages,
            TotalCount = pagedResponse.TotalCount
        };
        
        return newPagedResponse;
    }

    public async Task<GenreDetailsDto> CreateGenre(GenreCreationDto genreCreationDto)
    {
        _logger.LogInformation($"Creating new genre");
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = genreCreationDto.Name,
            Slug = genreCreationDto.Slug,
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
        if (genreUpdateDto.ParentGenreId != null && genreUpdateDto.ParentGenreId != Guid.Empty)
            genreToUpdate.ParentGenreId = genreUpdateDto.ParentGenreId;
        if (genreUpdateDto.ParentGenreId == Guid.Empty && genreToUpdate.ParentGenreId != null)
            genreToUpdate.ParentGenreId = null;
        if (genreUpdateDto.Slug != null) genreToUpdate.Slug = genreUpdateDto.Slug;

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