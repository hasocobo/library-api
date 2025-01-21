using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class BorrowedBook
{
    public Guid BorrowedBookId { get; init; }
    
    public DateTime BorrowedDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public DateTime ReturnedDate { get; set; }

    public bool IsReturned { get; set; } = false;
    [MaxLength(128)]
    public string BorrowerId { get; set; } = Guid.Empty.ToString();
    public ApplicationUser? Borrower { get; set; }
    
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
}