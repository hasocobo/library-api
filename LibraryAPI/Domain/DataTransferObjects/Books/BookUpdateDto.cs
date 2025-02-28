﻿using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.DataTransferObjects.Books;

public record BookUpdateDto
{
    [MaxLength(50)]
    public string? Title { get; init; }
    
    public Guid? AuthorId { get; init; }
    public string? ImageUrl { get; init; }
    
    public Guid? GenreId { get; init; }
    
    public int? PageCount { get; init; }
    
    public int? Quantity { get; init; }
}