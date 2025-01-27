using ExcelExportService;
using LibraryAPI.Application.Repositories;
using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IBorrowedBookRepository, BorrowedBookRepository>();
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LibraryDB")));

var host = builder.Build();
host.Run();