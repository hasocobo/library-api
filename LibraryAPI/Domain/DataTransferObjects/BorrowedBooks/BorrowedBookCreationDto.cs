namespace LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;

public record BorrowedBookCreationDto
{
    public DateTime? DueDate { get; init; }
    public Guid BookId { get; init; }
}