using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails
{
    public class UpdateSalesReturnDetailsCommandValidator:AbstractValidator<UpdateSalesReturnDetailsCommand>
    {
        public UpdateSalesReturnDetailsCommandValidator()
        {

            RuleFor( x => x.salesDetailsId).NotEmpty().WithMessage("sales return details is required");
        
        
        }
    }
}
