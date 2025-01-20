namespace LibraryAPI.Domain.Entities;

public class BorrowedBook
{
    public Guid BorrowedBookId { get; set; }
    
    public DateTime BorrowedDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public DateTime ReturnedDate { get; set; }

    public bool IsReturned { get; set; } = false;
        
    public Guid BorrowerId { get; set; }
    public ApplicationUser? Borrower { get; set; }
    
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
}