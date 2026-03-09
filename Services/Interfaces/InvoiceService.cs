using Invoicer.DTOs;
using Invoicer.Models;

namespace Invoicer.Services.Interfaces;

public interface IInvoiceService
{
    Task<Invoice> Create(InvoiceDto dto);
    Task<Invoice?> Update(int id, InvoiceDto dto);
    Task<bool> Delete(int id);
    Task<bool> Archive(int id);
    Task<Invoice?> GetById(int id);
    Task<object> GetAll(PaginationQuery query);
    Task<bool> ChangeStatus(int id, InvoiceStatus status);
}
