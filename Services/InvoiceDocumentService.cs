using Invoicer.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;

namespace Invoicer.Services;

public class InvoiceDocumentService
{
    public byte[] GeneratePdf(Invoice invoice)
    {
        using var stream = new MemoryStream();

        var writer = new PdfWriter(stream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph("INVOICE").SetFontSize(20));

        document.Add(new Paragraph($"Invoice ID: {invoice.Id}"));
        document.Add(new Paragraph($"Customer ID: {invoice.CustomerId}"));
        document.Add(new Paragraph($"Start Date: {invoice.StartDate}"));
        document.Add(new Paragraph($"End Date: {invoice.EndDate}"));
        document.Add(new Paragraph($"Status: {invoice.Status}"));

        document.Add(new Paragraph(" "));

        var table = new Table(4);

        table.AddHeaderCell("Service");
        table.AddHeaderCell("Quantity");
        table.AddHeaderCell("Rate");
        table.AddHeaderCell("Sum");

        foreach (var row in invoice.Rows)
        {
            table.AddCell(row.Service);
            table.AddCell(row.Quantity.ToString());
            table.AddCell(row.Rate.ToString());
            table.AddCell(row.Sum.ToString());
        }

        document.Add(table);

        document.Add(new Paragraph($"Total Sum: {invoice.TotalSum}"));

        document.Close();

        return stream.ToArray();
    }
}
