using LibraryAPI.Application.Repositories;
using LibraryAPI.Application.Repositories.Interfaces;

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
    }
}