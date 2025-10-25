namespace BugStore.Requests.Products;

public record UpdateProductsRequest(
    string Title,
    string Description,
    string Slug,
    decimal Price
);