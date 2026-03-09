using FluentValidation;
using Invoicer.DTOs;

namespace Invoicer.Validators
{
    public class InvoiceRowValidator : AbstractValidator<InvoiceRowDto>
    {
        public InvoiceRowValidator()
        {
            RuleFor(x => x.Service)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.Rate)
                .GreaterThan(0);
        }
    }
}
