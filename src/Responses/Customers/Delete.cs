namespace BugStore.Responses.Customers;

public record DeleteCustomersResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime BirthDate
);