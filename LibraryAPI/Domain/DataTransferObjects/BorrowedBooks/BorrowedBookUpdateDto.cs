namespace LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;

public record BorrowedBookUpdateDto
{
    public DateTime? DueDate { get; init; }
    public bool? IsReturned { get; init; }
}