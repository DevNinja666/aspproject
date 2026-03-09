using System;

namespace Invoicer.Models
{
    public class InvoiceRow
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public string Service { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Sum => Quantity * Rate;
    }
}
