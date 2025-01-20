﻿using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetGenresAsync();
    Task<Genre> GetGenreByIdAsync(Guid id);
    
    void CreateGenre(Genre genre);
    void UpdateGenre(Genre genre);
    void DeleteGenre(Genre genre);
}