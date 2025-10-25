namespace BugStore.Responses.Customers;

public record GetByIdCustomersResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime BirthDate
);