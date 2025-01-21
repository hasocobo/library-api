namespace LibraryAPI.Domain.DataTransferObjects.Genres;

public record GenreUpdateDto
{
    public Guid? ParentGenreId { get; init; }
    public string? Name { get; init; }
}