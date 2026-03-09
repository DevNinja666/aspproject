using Microsoft.AspNetCore.Mvc;
using Invoicer.Services;
using Invoicer.DTOs;

namespace Invoicer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _service;

    public CustomersController(CustomerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить список клиентов (с пагинацией, фильтрацией и сортировкой)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var result = await _service.GetAll(query);
        return Ok(result);
    }

    /// <summary>
    /// Получить клиента по Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var customer = await _service.GetById(id);
        return Ok(customer);
    }

    /// <summary>
    /// Создать клиента
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CustomerDto dto)
    {
        var customer = await _service.Create(dto);
        return Ok(customer);
    }

    /// <summary>
    /// Обновить клиента
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerDto dto)
    {
        var customer = await _service.Update(id, dto);
        return Ok(customer);
    }

    /// <summary>
    /// Архивировать клиента
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}
