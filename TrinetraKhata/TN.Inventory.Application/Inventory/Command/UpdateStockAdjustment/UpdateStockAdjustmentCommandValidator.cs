using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Inventory;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment
{
    public class UpdateStockAdjustmentCommandValidator : AbstractValidator<UpdateStockAdjustmentCommand>
    {
        public UpdateStockAdjustmentCommandValidator()
        {
            RuleFor(x => x.itemId)
               .NotEmpty().WithMessage("itemId is required.");

            RuleFor(x => x.reason)
               .Must(reason => Enum.IsDefined(typeof(StockAdjustment.ReasonType), reason))
               .WithMessage("Invalid reason type provided.");
        }
    }
}
