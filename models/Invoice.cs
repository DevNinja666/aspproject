using System;
using System.Collections.Generic;

namespace Invoicer.Models
{
    public enum InvoiceStatus
    {
        Created,
        Sent,
        Received,
        Paid,
        Cancelled,
        Rejected
    }

    public class Invoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public List<InvoiceRow> Rows { get; set; } = new();

        public decimal TotalSum => Rows.Sum(r => r.Sum);

        public string? Comment { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Created;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
