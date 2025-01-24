﻿using LibraryAPI.Domain.DataTransferObjects.Authors;
using LibraryAPI.Domain.DataTransferObjects.Books;
using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.DataTransferObjects.Genres;
using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Extensions;

public static class MappingExtensions
{
    public static BookDetailsDto ToDetailsDto(this Book book)
    {
        return new BookDetailsDto
        {
            AuthorId = book.AuthorId,
            Id = book.Id,
            Title = book.Title,
            PageCount = book.PageCount,
            GenreName = book.Genre?.Name,
            ImageUrl = book.ImageUrl,
            PublishYear = book.PublishYear,
            Quantity = book.Quantity,
            AuthorName = book.Author?.FirstName + " " + book.Author?.LastName,
        };
    }

    public static AuthorDetailsDto ToDetailsDto(this Author author)
    {
        return new AuthorDetailsDto
        {
            Id = author.Id,
            ApplicationUserId = author.ApplicationUserId,
            FullName = $"{author.FirstName} {author.LastName}",
            DateOfBirth = author.DateOfBirth,
            DateOfDeath = author.DateOfDeath,
        };
    }

    public static GenreDetailsDto ToDetailsDto(this Genre genre)
    {
        return new GenreDetailsDto
        {
            Id = genre.Id,
            Name = genre.Name,
            ParentGenreId = genre.ParentGenreId
        };
    }

    public static BorrowedBookDetailsDto ToDetailsDto(this BorrowedBook borrowedBook)
    {
        return new BorrowedBookDetailsDto
        {
            Id = borrowedBook.Id,
            BookId = borrowedBook.BookId,
            BorrowerId = borrowedBook.BorrowerId,
            BorrowerName = $"{borrowedBook.Borrower?.FirstName} {borrowedBook.Borrower?.LastName}",
            BookName = borrowedBook.Book?.Title,
            AuthorName = $"{borrowedBook.Book?.Author?.FirstName} {borrowedBook.Book?.Author?.LastName}",
            IsReturned = borrowedBook.IsReturned,
            BorrowingDate = borrowedBook.BorrowedDate,
            ReturningDate = borrowedBook.ReturnedDate,
            DueDate = borrowedBook.DueDate,
            PenaltyPrice = borrowedBook.CalculatePenaltyPrice()
        };
    }
}