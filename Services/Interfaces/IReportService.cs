namespace Invoicer.Services.Interfaces;

public interface IReportService
{
    Task<object> CustomerStatistics(DateTime start, DateTime end);
    Task<object> ServiceStatistics(DateTime start, DateTime end);
    Task<object> InvoiceStatistics(DateTime start, DateTime end);
}
