using Invoicer.Data;
using Invoicer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> CustomerStatistics(DateTime start, DateTime end)
    {
        var data = await _context.Invoices
            .Where(x => x.CreatedAt >= start && x.CreatedAt <= end)
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Count = g.Count(),
                Total = g.Sum(x => x.TotalSum)
            })
            .ToListAsync();

        return data;
    }

    public async Task<object> ServiceStatistics(DateTime start, DateTime end)
    {
        var data = await _context.InvoiceRows
            .Where(x => x.Invoice.CreatedAt >= start && x.Invoice.CreatedAt <= end)
            .GroupBy(x => x.Service)
            .Select(g => new
            {
                Service = g.Key,
                Count = g.Count(),
                Total = g.Sum(x => x.Sum)
            })
            .ToListAsync();

        return data;
    }

    public async Task<object> InvoiceStatistics(DateTime start, DateTime end)
    {
        var data = await _context.Invoices
            .Where(x => x.CreatedAt >= start && x.CreatedAt <= end)
            .GroupBy(x => x.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        return data;
    }
}
