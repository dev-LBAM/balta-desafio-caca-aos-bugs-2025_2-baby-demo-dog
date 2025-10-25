using BugStore.Data;
using BugStore.Models;
using BugStore.Requests.Orders;
using BugStore.Responses.Orders;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BugStore.Handlers.Orders;

public class OrderHandler(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<GetByIdOrdersResponse?> GetById(GetByIdOrdersRequest request, CancellationToken ct)
    {
        var order = await _db.Orders
            .Where(o => o.Id == request.OrderId)
            .Select(o => new GetByIdOrdersResponse(
                o.Id,
                o.Customer.Id,
                o.Customer.Name,
                o.CreatedAt,
                o.UpdatedAt,
                o.Lines.Select(ol => new OrderLineResponse(
                    ol.Product.Id,
                    ol.Product.Title,
                    ol.Quantity,
                    ol.Total
                )).ToList()
            ))
            .FirstOrDefaultAsync(ct);

        if(order is null)
        {
            return null;
        }
        return order;
    }

    public async Task<CreateOrdersResponse> CreateAsync(CreateOrdersRequest request, CancellationToken ct)
    {
        // ---- Valida cliente ----
        var customer = await _db.Customers
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, ct) 
            ?? throw new InvalidOperationException("Cliente não encontrado.");

        // ---- Agrupa linhas por ProductId ----
        var groupedLines = request.Lines
            .GroupBy(l => l.ProductId)
            .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
            .ToList();

        if (groupedLines.Any(l => l.Quantity <= 0))
            throw new InvalidOperationException("Quantidade de produto deve ser maior que zero.");

        // ---- Busca produtos no banco ----
        var productIds = groupedLines.Select(l => l.ProductId).ToList();
        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(ct);

        if (products.Count != groupedLines.Count)
            throw new InvalidOperationException("Um ou mais produtos não foram encontrados.");

        // ---- Cria pedido ----
        Order order = new()
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Customer = customer,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = [.. groupedLines.Select(line =>
            {
                var product = products.First(p => p.Id == line.ProductId);
                return new OrderLine
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Product = product,
                    Quantity = line.Quantity,
                    Total = product.Price * line.Quantity,
                };
            })]
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);

        return new CreateOrdersResponse(
            OrderId: order.Id,
            CustomerId: order.CustomerId,
            CreatedAt: order.CreatedAt,
            Lines: [.. order.Lines.Select(l => new OrderLineResponse(
                ProductId: l.ProductId,
                ProductTitle: l.Product.Title,
                Quantity: l.Quantity,
                Total: l.Total
            ))]
        );
    }

}