namespace BugStore.Responses.Products;

public record GetByIdProductsResponse(
    Guid Id,
    string Title,
    string Description,
    string Slug,
    decimal Price
);