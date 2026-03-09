using Microsoft.AspNetCore.Mvc;
using Invoicer.Services;
using Invoicer.DTOs;

namespace Invoicer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly InvoiceService _service;

    public InvoicesController(InvoiceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить список инвойсов (пагинация, фильтрация, сортировка)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var result = await _service.GetAll(query);
        return Ok(result);
    }

    /// <summary>
    /// Получить инвойс по Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var invoice = await _service.GetById(id);
        return Ok(invoice);
    }

    /// <summary>
    /// Создать инвойс
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(InvoiceDto dto)
    {
        var invoice = await _service.Create(dto);
        return Ok(invoice);
    }

    /// <summary>
    /// Скачать инвойс в PDF
    /// </summary>
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> DownloadPdf(int id)
    {
        var file = await _service.GeneratePdf(id);

        return File(file,
            "application/pdf",
            $"invoice_{id}.pdf");
    }

    /// <summary>
    /// Скачать инвойс в DOCX
    /// </summary>
    [HttpGet("{id}/docx")]
    public async Task<IActionResult> DownloadDocx(int id)
    {
        var file = await _service.GenerateDocx(id);

        return File(file,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            $"invoice_{id}.docx");
    }
}
