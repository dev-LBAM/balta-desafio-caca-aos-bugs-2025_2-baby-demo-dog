namespace BugStore.Requests.Customers;

public record CreateCustomersRequest(
    string Name,
    string Email,
    string Phone,
    DateTime BirthDate
);