namespace BugStore.Responses.Orders;

public record CreateOrdersResponse(
    Guid OrderId,
    Guid CustomerId,
    DateTime CreatedAt,
    List<OrderLineResponse> Lines
);

public record OrderLineResponse(
    Guid ProductId,
    string ProductTitle,
    int Quantity,
    decimal Total
);