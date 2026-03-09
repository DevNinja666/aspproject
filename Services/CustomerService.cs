using Invoicer.Data;
using Invoicer.DTOs;
using Invoicer.Models;
using Invoicer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> Create(CustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer?> Update(int id, CustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return null;

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Address = dto.Address;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> Delete(int id)
    {
        var customer = await _context.Customers
            .Include(x => x.Invoices)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
            return false;

        if (customer.Invoices.Any(x => x.Status == InvoiceStatus.Sent))
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Archive(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return false;

        customer.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Customer?> GetById(int id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    }

    public async Task<object> GetAll(PaginationQuery query)
    {
        var q = _context.Customers
            .Where(x => x.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            q = q.Where(x => x.Name.Contains(query.Search));

        if (query.SortBy == "name")
            q = q.OrderBy(x => x.Name);

        var total = await q.CountAsync();

        var data = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new
        {
            total,
            query.Page,
            query.PageSize,
            data
        };
    }
}
