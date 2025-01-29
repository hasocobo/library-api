namespace LibraryAPI.Domain.QueryFeatures;

public record PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
}
