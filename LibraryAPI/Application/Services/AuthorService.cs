using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.Authors;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Extensions;

namespace LibraryAPI.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IRepositoryManager repositoryManager, ILogger<AuthorService> logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task<AuthorDetailsDto> GetAuthorByIdAsync(Guid authorId)
    {
        _logger.LogInformation($"Retrieving author with ID: {authorId}");
        var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(authorId);

        if (author == null)
        {
            throw new NotFoundException("Author", authorId);
        }

        _logger.LogInformation("Returning author details");
        var authorToReturn = author.ToDetailsDto();

        return authorToReturn;
    }

    public async Task<IEnumerable<AuthorDetailsDto>> GetAuthorsAsync()
    {
        _logger.LogInformation("Retrieving authors");
        var authors = await _repositoryManager.AuthorRepository.GetAuthorsAsync() as List<Author>;

        if (authors == null)
        {
            _logger.LogInformation("No authors found");
            return Array.Empty<AuthorDetailsDto>();
        }

        _logger.LogInformation("Returning author details");
        var authorsToReturn = authors.Select(a => a.ToDetailsDto());
        return authorsToReturn;
    }

    public async Task<AuthorDetailsDto> CreateAuthorAsync(AuthorCreationDto authorCreationDto)
    {
        _logger.LogInformation("Creating author");
        var author = new Author
        {
            Id = Guid.NewGuid(),
            FirstName = authorCreationDto.FirstName,
            LastName = authorCreationDto.LastName,
            DateOfBirth = authorCreationDto.DateOfBirth,
            DateOfDeath = authorCreationDto.DateOfDeath,
            ApplicationUserId = authorCreationDto.ApplicationUserId
        };

        _repositoryManager.AuthorRepository.CreateAuthor(author);
        await _repositoryManager.SaveAsync();

        var authorToReturn = author.ToDetailsDto();
        return authorToReturn;
    }

    public async Task UpdateAuthorAsync(Guid authorId, AuthorUpdateDto authorUpdateDto)
    {
        _logger.LogInformation($"Updating author with ID: {authorId}");
        var authorToUpdate = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(authorId);
        if (authorToUpdate == null)
            throw new NotFoundException("Author", authorId);

        if (authorUpdateDto.FirstName != null) authorToUpdate.FirstName = authorUpdateDto.FirstName;
        if (authorUpdateDto.LastName != null) authorToUpdate.LastName = authorUpdateDto.LastName;
        if (authorUpdateDto.DateOfBirth != null) authorToUpdate.DateOfBirth = authorUpdateDto.DateOfBirth;
        if (authorUpdateDto.DateOfDeath != null) authorToUpdate.DateOfDeath = authorUpdateDto.DateOfDeath;
        
        _repositoryManager.AuthorRepository.UpdateAuthor(authorToUpdate);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteAuthorAsync(Guid authorId)
    {
        var authorToDelete = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(authorId);
        if (authorToDelete == null)
            throw new NotFoundException("Author", authorId);
        
        _repositoryManager.AuthorRepository.DeleteAuthor(authorToDelete);
        await _repositoryManager.SaveAsync();
    }
}