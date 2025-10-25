namespace BugStore.Requests.Customers;

public record UpdateCustomersRequest(
    string? Name,
    string? Email,
    string? Phone,
    string? BirthDate
);