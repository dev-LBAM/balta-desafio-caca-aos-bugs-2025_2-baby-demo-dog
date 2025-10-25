namespace BugStore.Responses.Customers;

public record GetAllCustomersResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string BirthDate
);