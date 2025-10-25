using BugStore.Data;
using BugStore.Models;
using BugStore.Requests.Products;
using BugStore.Responses.Products;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Products;

public class ProductHandler(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<CreateProductsResponse> Create(CreateProductsRequest request, CancellationToken ct)
    {
        Product product = new()
        {
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return new CreateProductsResponse(
            product.Id,
            product.Title,
            product.Description,
            product.Slug,
            product.Price
        );
    }

    public async Task<List<GetAllProductsResponse>> GetAll(CancellationToken ct)
    {
        return await _db.Products
            .Select(p => new GetAllProductsResponse(
                p.Id,
                p.Title,
                p.Description,
                p.Slug,
                p.Price
            ))
            .ToListAsync(ct);
    }

    public async Task<GetByIdProductsResponse?> GetById(GetByIdProductsRequest request, CancellationToken ct)
    {
        Product? product = await _db.Products.FindAsync([request.Id, ct], cancellationToken: ct);
        if (product == null)
        {
            return null;
        }
        return new GetByIdProductsResponse(
            product.Id,
            product.Title,
            product.Description,
            product.Slug,
            product.Price
        );
    }

    public async Task<UpdateProductsResponse?> Update(Guid id, UpdateProductsRequest request, CancellationToken ct)
    {
        Product? product = await _db.Products.FindAsync([id, ct], cancellationToken: ct);
        if (product == null)
        {
            return null;
        }
        if (request.Title != null) product.Title = request.Title;
        if (request.Description != null) product.Description = request.Description;
        if (request.Slug != null) product.Slug = request.Slug;
        if (request.Price != 0) product.Price = request.Price;

        await _db.SaveChangesAsync(ct);
        return new UpdateProductsResponse(
            product.Id,
            product.Title,
            product.Description,
            product.Slug,
            product.Price
        );
    }

    public async Task<DeleteProductsResponse?> Delete(DeleteProductsRequest request, CancellationToken ct)
    {
        Product? product = await _db.Products.FindAsync([request.Id, ct], cancellationToken: ct);
        if (product == null)
        {
            return null;
        }
        _db.Products.Remove(product);
        await _db.SaveChangesAsync(ct);
        return new DeleteProductsResponse(
            product.Id,
            product.Title,
            product.Description,
            product.Slug,
            product.Price
        );
    }
}
