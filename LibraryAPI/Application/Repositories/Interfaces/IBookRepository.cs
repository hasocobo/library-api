﻿using System.Collections;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.QueryFeatures;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IBookRepository
{
    Task<PagedResponse<Book>> GetBooksAsync(QueryParameters queryParameters);
    Task<IEnumerable<Book>> GetDeletedBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book?> GetDeletedBookByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid id);
    Task<IEnumerable<Book>> GetBooksByGenreIdAsync(Guid id);
    
    Task<bool> CheckIfBookIsAvailableAsync(Guid bookId);
    
    void CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
    
}