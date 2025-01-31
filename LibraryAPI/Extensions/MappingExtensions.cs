using LibraryAPI.Domain.DataTransferObjects.Authors;
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
            BookId = book.Id,
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
            Slug = genre.Slug,
            ParentGenreId = genre.ParentGenreId,
            Books = genre.Books.Select(b => b.ToDetailsDto()).ToList()
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
            Title = borrowedBook.Book?.Title,
            Description = borrowedBook.Book?.Description,
            GenreName = borrowedBook.Book?.Genre?.Name,
            ImageUrl = borrowedBook.Book?.ImageUrl,
            PageCount = borrowedBook.Book!.PageCount,
            AuthorName = $"{borrowedBook.Book?.Author?.FirstName} {borrowedBook.Book?.Author?.LastName}",
            IsReturned = borrowedBook.IsReturned,
            BorrowingDate = borrowedBook.BorrowedDate,
            ReturningDate = borrowedBook.ReturnedDate,
            DueDate = borrowedBook.DueDate,
            BorrowStatus = borrowedBook.BorrowStatus,
            PenaltyPrice = borrowedBook.CalculatePenaltyPrice()
        };
    }
}