using LibraryAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Infrastructure;

public class LibraryContext : IdentityDbContext<ApplicationUser>
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BorrowedBook> BorrowedBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(book => book.Id);

            entity
                .HasMany(book => book.BorrowedBooks)
                .WithOne(borrowedBook => borrowedBook.Book)
                .HasForeignKey(borrowedBook => borrowedBook.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId);
        });


        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(author => author.Id);

            entity
                .HasMany(author => author.Books)
                .WithOne(book => book.Author)
                .HasForeignKey(book => book.AuthorId);
        });

        modelBuilder.Entity<BorrowedBook>(entity =>
        {
            entity.HasKey(borrowedBook => borrowedBook.Id);

            entity
                .HasOne(borrowedBook => borrowedBook.Book)
                .WithMany(book => book.BorrowedBooks)
                .HasForeignKey(borrowedBook => borrowedBook.BookId);

            entity
                .HasOne(borrowedBook => borrowedBook.Borrower)
                .WithMany(borrower => borrower.BorrowedBooks)
                .HasForeignKey(borrowedBook => borrowedBook.BorrowerId);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(applicationUser => applicationUser.Id);

            entity
                .HasOne(applicationUser => applicationUser.Author)
                .WithOne(author => author.User)
                .HasForeignKey<Author>(author => author.ApplicationUserId);

            entity
                .HasMany(user => user.BorrowedBooks)
                .WithOne(borrowedBook => borrowedBook.Borrower)
                .HasForeignKey(borrowedBook => borrowedBook.BorrowerId);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(genre => genre.Id);

            entity
                .HasMany(genre => genre.Books)
                .WithOne(book => book.Genre)
                .HasForeignKey(book => book.GenreId);

            entity
                .HasMany(genre => genre.SubGenres)
                .WithOne(subGenre => subGenre.ParentGenre)
                .HasForeignKey(subGenre => subGenre.ParentGenreId);
        });

        modelBuilder.Entity<IdentityRole>()
            .HasData
            (
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "3d09660f-c1e2-4f45-878c-5d19adcfc44d"},
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "1894d025-0243-4892-b115-56c333fb9a2d"},
                new IdentityRole { Id = "3", Name = "Librarian", NormalizedName = "LIBRARIAN", ConcurrencyStamp = "a84be136-d8b4-4cfe-aae3-70c30b620527"}
            );
    }
}