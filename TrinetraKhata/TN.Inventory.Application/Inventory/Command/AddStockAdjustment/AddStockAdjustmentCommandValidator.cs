using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TN.Shared.Domain.Entities.Inventory;

namespace TN.Inventory.Application.Inventory.Command.AddStockAdjustment
{
    public class AddStockAdjustmentCommandValidator:AbstractValidator<AddStockAdjustmentCommand>
    {
        public AddStockAdjustmentCommandValidator() 
        {
            RuleFor(RuleFor => RuleFor.itemId)
                .NotEmpty().WithMessage("Item id is required.");

            RuleFor(x => x.reason)
             .Must(reason => Enum.IsDefined(typeof(StockAdjustment.ReasonType), reason))
             .WithMessage("Invalid reason type provided.");
        }
    }
}
