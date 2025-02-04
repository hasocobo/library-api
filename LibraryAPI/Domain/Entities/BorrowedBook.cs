using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class BorrowedBook
{
    public Guid Id { get; init; }

    public DateTime BorrowedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public bool? IsReturned { get; set; } = false;
    
    public BorrowStatus BorrowStatus { get; set; } = BorrowStatus.Pending;
    [MaxLength(128)] public string BorrowerId { get; set; } = Guid.Empty.ToString();
    public ApplicationUser? Borrower { get; set; }

    public Guid BookId { get; set; }
    public Book? Book { get; set; }

    public int CalculatePenaltyPrice()
    {
        int penaltyPrice = 0;
        int timeSpan = 0;
        // TODO: remove isReturned, no need for it
        if (IsReturned == null || IsReturned == false || ReturnedDate == null)
            timeSpan = DateTime.UtcNow.Subtract(BorrowedDate).Days;
        
        if (IsReturned == true && ReturnedDate != null)
            timeSpan = ReturnedDate!.Value.Subtract(BorrowedDate).Days;

        if (timeSpan <= 1)
            penaltyPrice = 0;
        else
            penaltyPrice = timeSpan * 10;

        return penaltyPrice;
    }
}