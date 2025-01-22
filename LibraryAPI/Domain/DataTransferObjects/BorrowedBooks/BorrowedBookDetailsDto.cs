namespace LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;

public record BorrowedBookDetailsDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public string? BookName { get; init; } = string.Empty;
    public string? AuthorName { get; init; } = string.Empty;
    public required string BorrowerId { get; init; } 
    public string? BorrowerName { get; init; } = string.Empty;
    public DateTime BorrowingDate { get; init; } = DateTime.UtcNow;
    public bool? IsReturned { get; init; }
    public DateTime? ReturningDate { get; init; }
    public DateTime? DueDate { get; init; }
    
    public int PenaltyPrice { get; init; }
}