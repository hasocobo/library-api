using LibraryAPI.Domain.DataTransferObjects.Books;
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
        };
    }
}