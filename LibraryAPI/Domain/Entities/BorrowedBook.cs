using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Entities;

public class BorrowedBook
{
    public Guid Id { get; init; }

    public DateTime BorrowedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public bool? IsReturned { get; set; } = false;
    [MaxLength(128)] public string BorrowerId { get; set; } = Guid.Empty.ToString();
    public ApplicationUser? Borrower { get; set; }

    public Guid BookId { get; set; }
    public Book? Book { get; set; }

    public int CalculatePenaltyPrice()
    {
        int penaltyPrice = 0;
        int timeSpan = IsReturned == false ?
            DateTime.UtcNow.Subtract(BorrowedDate).Minutes :
            ReturnedDate!.Value.Subtract(BorrowedDate).Minutes;

        if (timeSpan <= 10)
            penaltyPrice = 0;
        else
            penaltyPrice = (timeSpan - 10) * 10;

        return penaltyPrice;
    }
}