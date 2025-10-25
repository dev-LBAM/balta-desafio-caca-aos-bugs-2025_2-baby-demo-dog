namespace BugStore.Requests.Products;

public record CreateProductsRequest(
    string Title,
    string Description,
    string Slug,
    decimal Price
);