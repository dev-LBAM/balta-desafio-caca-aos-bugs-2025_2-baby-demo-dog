namespace BugStore.Requests.Orders;

public record CreateOrdersRequest(
    Guid CustomerId,
    List<CreateOrderLineRequest> Lines
);

public record CreateOrderLineRequest(
    Guid ProductId,
    int Quantity
);