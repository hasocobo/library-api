namespace LibraryAPI.Domain.DataTransferObjects.Books;

public record BookDetailsDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? AuthorName { get; init; }
    public Guid AuthorId { get; init; }
    public DateTime PublishDate { get; init; }
    public int PageCount { get; init; }
};