using BugStore.Data;
using BugStore.Handlers.Customers;
using BugStore.Models;
using BugStore.Requests.Customers;
using BugStore.Responses.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string cnnString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string não encontrada.");

// Registrar DbContext no container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(cnnString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


// Customers Endpoints
app.MapGet("/v1/customers", async (AppDbContext db)  =>
{
    var handler = new CustomerHandler(db);
    List<GetAllCustomersResponse> result = await handler.GetAll();
    return Results.Ok(result);
});

app.MapGet("/v1/customers/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var handler = new CustomerHandler(db);

    GetByIdCustomersRequest request = new(Id: id);
    GetByIdCustomersResponse? result =  await handler.GetById(request);

    if (result == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
});

app.MapPost("/v1/customers", async (CreateCustomersRequest req, AppDbContext db) =>
{
    var handler = new CustomerHandler(db);
    CreateCustomerResponse result =  await handler.Create(req);
    return Results.Created($"/v1/customers/{result.Id}", result);
});

app.MapPut("/v1/customers/{id}", async ([FromRoute] Guid id, UpdateCustomersRequest request, AppDbContext db) =>
{
    var handler = new CustomerHandler(db);
    if (request.Name == null && 
    request.Email == null && 
    request.Phone == null && 
    request.BirthDate == null) return Results.NoContent();

    UpdateCustomersResponse? result =  await handler.Update(id, request);

    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

app.MapDelete("/v1/customers/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var handler = new CustomerHandler(db);
    DeleteCustomersRequest request = new(Id: id);
    DeleteCustomersResponse? result =  await handler.Delete(request);
    if(result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(result);
});

// Products Endpoints
app.MapGet("/v1/products", () => "Hello World!");
app.MapGet("/v1/products/{id}", () => "Hello World!");
app.MapPost("/v1/products", () => "Hello World!");
app.MapPut("/v1/products/{id}", () => "Hello World!");
app.MapDelete("/v1/products/{id}", () => "Hello World!");

// Orders Endpoints
app.MapGet("/v1/orders/{id}", () => "Hello World!");
app.MapPost("/v1/orders", () => "Hello World!");

app.Run();
