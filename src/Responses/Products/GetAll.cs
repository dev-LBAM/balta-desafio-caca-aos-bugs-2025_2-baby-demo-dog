namespace BugStore.Responses.Products;

public record GetAllProductsResponse(
    Guid Id,
    string Title,
    string Description,
    string Slug,
    decimal Price
);