﻿namespace LibraryAPI.Domain.DataTransferObjects.Books;

public record BookDetailsDto
{
    public Guid BookId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? AuthorName { get; init; }
    public Guid AuthorId { get; init; }
    public int PublishYear { get; init; }
    public int PageCount { get; init; }
    public Guid GenreId { get; init; }
    public string? GenreName { get; init; }
    public string? ImageUrl { get; init; }
    
    public string? Description { get; init; }
    public int Quantity { get; init; }
};