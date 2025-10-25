using BugStore.Data;
using BugStore.Models;
using BugStore.Requests.Customers;
using BugStore.Responses.Customers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Customers;

public class CustomerHandler(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<CreateCustomerResponse> Create(CreateCustomersRequest request)
    {
        Customer customerCreate = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate
        };

        _db.Customers.Add(customerCreate);
        await  _db.SaveChangesAsync();

        CreateCustomerResponse customerResponse = new
        (
            Id: customerCreate.Id,
            Name: customerCreate.Name,
            Email: customerCreate.Email,
            Phone: customerCreate.Phone,
            BirthDate: customerCreate.BirthDate.ToString("yyyy-MM-dd")
        );

        return customerResponse;
    }

    public async Task<List<GetAllCustomersResponse>> GetAll()
    {
        return await _db.Customers
            .Select(c => new GetAllCustomersResponse(
                c.Id,
                c.Name,
                c.Email,
                c.Phone,
                c.BirthDate.ToString("yyyy-MM-dd")
            ))
            .ToListAsync();
    }

    public async Task<GetByIdCustomersResponse?> GetById(GetByIdCustomersRequest request)
    {
        Customer? customer = await _db.Customers.FindAsync(request.Id);

        if (customer == null)
        {
            return null;
        }

        return new GetByIdCustomersResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.BirthDate
        );
    }

    public async Task<DeleteCustomersResponse?> Delete(DeleteCustomersRequest request)
    {
        Customer? customer = await _db.Customers.FindAsync(request.Id);
        if (customer == null)
        {
            return null;
            
        }
        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
        return new DeleteCustomersResponse
        (
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.BirthDate
        );
    }

    public async Task<UpdateCustomersResponse?> Update(Guid id,UpdateCustomersRequest request)
    {
        Customer? customer = await _db.Customers.FindAsync(id);
        if (customer == null)
        {
            return null;
        }
        customer.Name = request.Name ?? customer.Name;
        customer.Email = request.Email ?? customer.Email;
        customer.Phone = request.Phone ?? customer.Phone;

        if (request.BirthDate != null)
        {
            customer.BirthDate = DateTime.Parse(request.BirthDate);
        }

        await _db.SaveChangesAsync();
        return new UpdateCustomersResponse
        (
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.BirthDate
        );
    }
}