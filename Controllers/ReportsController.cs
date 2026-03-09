using Microsoft.AspNetCore.Mvc;
using Invoicer.Services;

namespace Invoicer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ReportService _service;

    public ReportsController(ReportService service)
    {
        _service = service;
    }

    /// <summary>
    /// Статистика по клиентам
    /// </summary>
    [HttpGet("clients")]
    public async Task<IActionResult> ClientStats(DateTime start, DateTime end)
    {
        var result = await _service.GetClientStats(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Статистика по работам
    /// </summary>
    [HttpGet("works")]
    public async Task<IActionResult> WorkStats(DateTime start, DateTime end)
    {
        var result = await _service.GetWorkStats(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Статистика по статусам инвойсов
    /// </summary>
    [HttpGet("invoice-status")]
    public async Task<IActionResult> InvoiceStats(DateTime start, DateTime end)
    {
        var result = await _service.GetInvoiceStats(start, end);
        return Ok(result);
    }
}
