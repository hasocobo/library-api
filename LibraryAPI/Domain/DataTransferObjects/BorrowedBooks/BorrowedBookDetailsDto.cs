namespace LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;

public record BorrowedBookDetailsDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public string? BookName { get; init; } = string.Empty;
    public string? AuthorName { get; init; } = string.Empty;
    public required string BorrowerId { get; init; } 
    public DateTime BorrowedDate { get; init; }
    public bool? IsReturned { get; init; }
    public DateTime? ReturnedDate { get; init; }
    public DateTime? DueDate { get; init; }
    
    public int PenaltyPrice { get; init; }
}