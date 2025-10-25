using BugStore.Data;
using BugStore.Handlers.Customers;
using BugStore.Handlers.Products;
using BugStore.Models;
using BugStore.Requests.Customers;
using BugStore.Requests.Products;
using BugStore.Responses.Customers;
using BugStore.Responses.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

string cnnString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string não encontrada.");

// Registrar DbContext no container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(cnnString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


// Customers Endpoints
app.MapGet("/v1/customers", async (AppDbContext db, CancellationToken ct)  =>
{
    var handler = new CustomerHandler(db);
    List<GetAllCustomersResponse> result = await handler.GetAll(ct);
    return Results.Ok(result);
});

app.MapGet("/v1/customers/{id}", async ([FromRoute] Guid id, AppDbContext db, CancellationToken ct) =>
{
    var handler = new CustomerHandler(db);

    GetByIdCustomersRequest request = new(Id: id);
    GetByIdCustomersResponse? result =  await handler.GetById(request, ct);

    if (result == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
});

app.MapPost("/v1/customers", async (CreateCustomersRequest req, AppDbContext db, CancellationToken ct) =>
{
    var handler = new CustomerHandler(db);
    CreateCustomerResponse result =  await handler.Create(req, ct);
    return Results.Created($"/v1/customers/{result.Id}", result);
});

app.MapPut("/v1/customers/{id}", async ([FromRoute] Guid id, UpdateCustomersRequest request, AppDbContext db, CancellationToken ct) =>
{
    var handler = new CustomerHandler(db);
    if (request.Name == null && 
    request.Email == null && 
    request.Phone == null && 
    request.BirthDate == null) return Results.NoContent();

    UpdateCustomersResponse? result =  await handler.Update(id, request, ct);

    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

app.MapDelete("/v1/customers/{id}", async ([FromRoute] Guid id, AppDbContext db, CancellationToken ct) =>
{
    var handler = new CustomerHandler(db);
    DeleteCustomersRequest request = new(Id: id);
    DeleteCustomersResponse? result =  await handler.Delete(request, ct);
    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

// Products Endpoints
app.MapGet("/v1/products", async (AppDbContext db, CancellationToken ct) =>
{ 
    var handler = new ProductHandler(db);
    List<GetAllProductsResponse> result = await handler.GetAll(ct);
    return Results.Ok(result);
});

app.MapGet("/v1/products/{id}", async ([FromRoute] Guid id, AppDbContext db, CancellationToken ct) =>
{
    var handler = new ProductHandler(db);
    GetByIdProductsRequest request = new(Id: id);
    GetByIdProductsResponse? result = await handler.GetById(request, ct);
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

app.MapPost("/v1/products", async(CreateProductsRequest request, AppDbContext db, CancellationToken ct) =>
{
    var handler = new ProductHandler(db);
    CreateProductsResponse result = await handler.Create(request, ct);
    return Results.Created($"/v1/products/{result.Id}", result);
});

app.MapPut("/v1/products/{id}", async ([FromRoute] Guid id, UpdateProductsRequest request, AppDbContext db, CancellationToken ct) =>
{
    var handler = new ProductHandler(db);
    if (request.Title == null && 
    request.Description == null &&
    request.Slug == null &&
    request.Price == 0) return Results.NoContent();

    UpdateProductsResponse? result =  await handler.Update(id, request, ct);
    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});
app.MapDelete("/v1/products/{id}", async ([FromRoute] Guid id, AppDbContext db, CancellationToken ct) =>
{
    var handler = new ProductHandler(db);
    DeleteProductsRequest request = new(Id: id);
    DeleteProductsResponse? result =  await handler.Delete(request, ct);
    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);

});

// Orders Endpoints
app.MapGet("/v1/orders/{id}", () => "Hello World!");
app.MapPost("/v1/orders", () => "Hello World!");

app.Run();
