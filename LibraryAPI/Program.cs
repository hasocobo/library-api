using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using LibraryAPI.Domain.Exceptions;
using LibraryAPI.Extensions;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.Host.UseSerilog();
    builder.Services.AddLogging();

    builder.Services.AddDbContext<LibraryContext>(options =>
    { 
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "library.db");
        options.UseSqlite($"Data Source={dbPath}");
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddRateLimiter(_ => _
        .AddFixedWindowLimiter(policyName: "fixed", options =>
        {
            options.PermitLimit = 4;
            options.Window = TimeSpan.FromSeconds(12);
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 2;
        }));
    builder.Services.AddSwaggerGen();
    builder.Services.AddEntityRepositories();
    builder.Services.AddEntityServices();
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });

    builder.Services.ConfigureCors();
    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJwtAuthentication(builder.Configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("Librarian", policy => policy.RequireRole("Admin", "Librarian"));
        
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser() 
            .Build();
    });
}

var app = builder.Build();

{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    app.ConfigureExceptionHandler(logger);

    if (app.Environment.IsProduction())
    {
        app.UseHsts();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    
    app.UseRateLimiter();

    app.UseRouting();

    app.UseCors("CorsPolicy");
    
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();