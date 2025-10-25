namespace BugStore.Responses.Orders;

public record GetByIdOrdersResponse(
    Guid OrderId,
    Guid CustomerId,
    string CustomerName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<OrderLineResponse> Lines
);