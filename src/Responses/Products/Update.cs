namespace BugStore.Responses.Products;

public record UpdateProductsResponse(
    Guid Id,
    string Title,
    string Description,
    string Slug,
    decimal Price
);