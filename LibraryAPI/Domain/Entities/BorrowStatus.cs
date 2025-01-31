namespace LibraryAPI.Domain.Entities;

public enum BorrowStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = -1,
}