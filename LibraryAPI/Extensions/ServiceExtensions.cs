using LibraryAPI.Application.Repositories;
using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services;
using LibraryAPI.Application.Services.Interfaces;

namespace LibraryAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }

    public static void AddEntityRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBorrowedBookRepository, BorrowedBookRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void AddEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBorrowedBookService, BorrowedBookService>();
        services.AddScoped<IServiceManager, ServiceManager>();
    }
}