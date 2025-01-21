namespace LibraryAPI.Application.Services.Interfaces;

public interface IServiceManager
{
    IGenreService GenreService { get; }
    IBookService BookService { get; }
    IBorrowedBookService BorrowedBookService { get; }
    IAuthorService AuthorService { get; }
}