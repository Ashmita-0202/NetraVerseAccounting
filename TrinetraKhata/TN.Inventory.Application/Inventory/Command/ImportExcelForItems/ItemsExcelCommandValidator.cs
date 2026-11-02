using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.ImportExcelForItems
{
    public class ItemsExcelCommandValidator : AbstractValidator<ItemsExcelCommand>
    {
        public ItemsExcelCommandValidator()
        {
            RuleFor(x => x.formFile)
                .NotEmpty().WithMessage("item name is required.");
        }
    }
}
