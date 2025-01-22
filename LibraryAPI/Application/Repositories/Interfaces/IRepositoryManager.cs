using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IRepositoryManager
{
    IAuthorRepository AuthorRepository { get; }
    IGenreRepository GenreRepository { get; }
    IBookRepository BookRepository { get; }
    IBorrowedBookRepository BorrowedBookRepository { get; }
    
    Task SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}