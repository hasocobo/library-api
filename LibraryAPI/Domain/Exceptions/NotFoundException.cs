namespace LibraryAPI.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid entityId) : base($"{entityName} with ID {entityId} was not found.")
    {
        
    }
}