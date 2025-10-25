namespace BugStore.Responses.Products;

public record DeleteProductsResponse(
    Guid Id,
    string Title,
    string Description,
    string Slug,
    decimal Price
);