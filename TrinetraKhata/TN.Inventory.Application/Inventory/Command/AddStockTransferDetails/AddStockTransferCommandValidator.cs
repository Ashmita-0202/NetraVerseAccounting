using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockTransferDetails
{
    public class AddStockTransferCommandValidator : AbstractValidator<AddStockTransferCommand>
    {
        public AddStockTransferCommandValidator()
        {
            RuleFor(x => x.transferDate)
                 .NotEmpty().WithMessage("Date is required.");
        }
    }
}
