namespace BugStore.Responses.Customers;

public record CreateCustomerResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string BirthDate
);