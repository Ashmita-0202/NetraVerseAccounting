using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.UpdateItemGroup
{
    public class UpdateItemGroupCommandValidator : AbstractValidator<UpdateItemGroupCommand>
    {
        public UpdateItemGroupCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("itemGroup name is required.");
        }
    }
}
