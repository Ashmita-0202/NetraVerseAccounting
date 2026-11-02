using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails
{
    public  class AddSalesReturnDetailsCommandValidator:AbstractValidator<AddSalesReturnDetailsCommand>
    {
        public AddSalesReturnDetailsCommandValidator()
        {
            RuleFor(x=>x.totalReturnAmount).NotEmpty().WithMessage("total return amount is required");
        }
    }
}
