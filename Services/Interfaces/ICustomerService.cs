using Invoicer.DTOs;
using Invoicer.Models;

namespace Invoicer.Services.Interfaces;

public interface ICustomerService
{
    Task<Customer> Create(CustomerDto dto);
    Task<Customer?> Update(int id, CustomerDto dto);
    Task<bool> Delete(int id);
    Task<bool> Archive(int id);
    Task<Customer?> GetById(int id);
    Task<object> GetAll(PaginationQuery query);
}
