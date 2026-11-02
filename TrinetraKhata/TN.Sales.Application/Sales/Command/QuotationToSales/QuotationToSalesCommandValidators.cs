using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.QuotationToSales
{
    public class QuotationToSalesCommandValidators : AbstractValidator<QuotationToSalesCommand>
    {
        public QuotationToSalesCommandValidators()
        {
            RuleFor(x => x.salesQuotationId)
                .NotEmpty().WithMessage("Sales Quotation Id must be required.");
            RuleFor(x => x.paymentId)
                .NotEmpty().WithMessage("Payment Id must be required.");
        }
    }
}
