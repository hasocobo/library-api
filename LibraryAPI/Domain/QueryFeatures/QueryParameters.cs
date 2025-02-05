using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.QueryFeatures;

public record QueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 6;

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? SearchTerm { get; set; }
    
    public Guid? GenreId { get; set; }
    public Guid? AuthorId { get; set; }
    public bool? SortDescending { get; set; }
}