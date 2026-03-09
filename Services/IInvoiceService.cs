using Invoicer.Data;
using Invoicer.DTOs;
using Invoicer.Models;
using Invoicer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Services;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _context;

    public InvoiceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice> Create(InvoiceDto dto)
    {
        var rows = dto.Rows.Select(r => new InvoiceRow
        {
            Service = r.Service,
            Quantity = r.Quantity,
            Rate = r.Rate,
            Sum = r.Quantity * r.Rate
        }).ToList();

        var total = rows.Sum(x => x.Sum);

        var invoice = new Invoice
        {
            CustomerId = dto.CustomerId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Comment = dto.Comment,
            Rows = rows,
            TotalSum = total,
            Status = InvoiceStatus.Created,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return invoice;
    }

    public async Task<Invoice?> Update(int id, InvoiceDto dto)
    {
        var invoice = await _context.Invoices
            .Include(x => x.Rows)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice == null || invoice.Status != InvoiceStatus.Created)
            return null;

        _context.InvoiceRows.RemoveRange(invoice.Rows);

        var rows = dto.Rows.Select(r => new InvoiceRow
        {
            Service = r.Service,
            Quantity = r.Quantity,
            Rate = r.Rate,
            Sum = r.Quantity * r.Rate
        }).ToList();

        invoice.Rows = rows;
        invoice.TotalSum = rows.Sum(x => x.Sum);
        invoice.Comment = dto.Comment;
        invoice.StartDate = dto.StartDate;
        invoice.EndDate = dto.EndDate;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return invoice;
    }

    public async Task<bool> Delete(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null || invoice.Status != InvoiceStatus.Created)
            return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Archive(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Invoice?> GetById(int id)
    {
        return await _context.Invoices
            .Include(x => x.Rows)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    }

    public async Task<object> GetAll(PaginationQuery query)
    {
        var q = _context.Invoices
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Rows)
            .AsQueryable();

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

    public async Task<bool> ChangeStatus(int id, InvoiceStatus status)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return false;

        invoice.Status = status;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
