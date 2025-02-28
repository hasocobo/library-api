﻿using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class Book
{
    public Guid Id { get; init; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public int PageCount { get; set; }

    public int PublishYear { get; set; }
    
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }
    
    public Guid GenreId { get; set; }
    public Genre? Genre { get; set; }

    public int Quantity { get; set; } = 1;
    [MaxLength(100)]
    public string? ImageUrl { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    [MaxLength(200)]
    public string? Description { get; set; }
    public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
}