using Invoicer.Data;
using Invoicer.DTOs;
using Invoicer.Models;
using Invoicer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly InvoiceDocumentService _documentService;

        public InvoicesController(AppDbContext context, InvoiceDocumentService documentService)
        {
            _context = context;
            _documentService = documentService;
        }

        /// <summary>
        /// Получить список инвойсов с пагинацией, фильтром и сортировкой
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            var invoices = _context.Invoices
                .Include(i => i.Rows)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                invoices = invoices.Where(i => i.Comment.Contains(query.Search));
            }

            if (query.SortBy == "startDate")
                invoices = invoices.OrderBy(i => i.StartDate);
            else if (query.SortBy == "endDate")
                invoices = invoices.OrderBy(i => i.EndDate);
            else
                invoices = invoices.OrderBy(i => i.Id);

            var total = await invoices.CountAsync();

            var result = await invoices
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return Ok(new
            {
                total,
                page = query.Page,
                pageSize = query.PageSize,
                data = result
            });
        }

        /// <summary>
        /// Получить инвойс по Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        /// <summary>
        /// Создать инвойс
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Invoice invoice)
        {
            invoice.TotalSum = invoice.Rows.Sum(r => r.Sum);
            invoice.CreatedAt = DateTimeOffset.UtcNow;
            invoice.UpdatedAt = DateTimeOffset.UtcNow;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }

        /// <summary>
        /// Редактировать инвойс (только если статус Created)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Invoice invoiceUpdate)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            if (invoice.Status != InvoiceStatus.Created)
                return BadRequest("Можно редактировать только инвойсы со статусом Created.");

            invoice.StartDate = invoiceUpdate.StartDate;
            invoice.EndDate = invoiceUpdate.EndDate;
            invoice.Comment = invoiceUpdate.Comment;
            invoice.Rows = invoiceUpdate.Rows;
            invoice.TotalSum = invoice.Rows.Sum(r => r.Sum);
            invoice.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(invoice);
        }

        /// <summary>
        /// Изменить статус инвойса
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] InvoiceStatus status)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return NotFound();

            invoice.Status = status;
            invoice.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(invoice);
        }

        /// <summary>
        /// Архивировать инвойс (soft delete)
        /// </summary>
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> Archive(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return NotFound();

            invoice.DeletedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Удалить инвойс (hard delete, только если статус Created)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();

            if (invoice.Status != InvoiceStatus.Created)
                return BadRequest("Можно удалить только инвойсы со статусом Created.");

            _context.InvoiceRows.RemoveRange(invoice.Rows);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Скачать инвойс в PDF
        /// </summary>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            var pdf = _documentService.GeneratePdf(invoice);
            return File(pdf, "application/pdf", $"invoice_{id}.pdf");
        }
    }
}
