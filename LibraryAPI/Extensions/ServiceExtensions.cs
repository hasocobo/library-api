using System.Text;
using LibraryAPI.Application.Repositories;
using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Application.Services;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IServiceManager, ServiceManager>();
    }
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<LibraryContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("DOTNETJWTSECRET");

        if (string.IsNullOrWhiteSpace(secretKey)) throw new ApplicationException("Missing JWT token secret");
        
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });
    }
}