using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryAPI.Application.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly LibraryContext _libraryContext;
    private readonly IGenreRepository _genreRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBorrowedBookRepository _borrowedBookRepository;

    public RepositoryManager(LibraryContext libraryContext, IGenreRepository genreRepository,
        IBookRepository bookRepository, IAuthorRepository authorRepository,
        IBorrowedBookRepository borrowedBookRepository)
    {
        _libraryContext = libraryContext;
        _genreRepository = genreRepository;
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _borrowedBookRepository = borrowedBookRepository;
    }
    
    public async Task SaveAsync() => await _libraryContext.SaveChangesAsync();
    public async Task<IDbContextTransaction> BeginTransactionAsync() =>
        await _libraryContext.Database.BeginTransactionAsync();
    
    public IGenreRepository GenreRepository => _genreRepository;
    public IBookRepository BookRepository => _bookRepository;
    public IAuthorRepository AuthorRepository => _authorRepository;
    public IBorrowedBookRepository BorrowedBookRepository => _borrowedBookRepository;
    
}