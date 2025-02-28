﻿using LibraryAPI.Application.Services.Interfaces;

namespace LibraryAPI.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;
    private readonly IGenreService _genreService;
    private readonly IBorrowedBookService _borrowedBookService;
    private readonly IAuthService _authService;

    public ServiceManager(IBookService bookService, IAuthorService authorService, IGenreService genreService,
        IBorrowedBookService borrowedBookService, IAuthService authService)
    {
        _bookService = bookService;
        _authorService = authorService;
        _genreService = genreService;
        _authService = authService;
        _borrowedBookService = borrowedBookService;
    }
    
    public IBookService BookService => _bookService;
    public IAuthorService AuthorService => _authorService;
    public IGenreService GenreService => _genreService;
    public IBorrowedBookService BorrowedBookService => _borrowedBookService;
    public IAuthService AuthService => _authService;
}