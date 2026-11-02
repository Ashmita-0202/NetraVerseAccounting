using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.AddItemGroup
{
   public class AddItemGroupCommandValidator:AbstractValidator<AddItemGroupCommand>
    {
        public AddItemGroupCommandValidator() 
        {
            RuleFor(x => x.name)
                  .NotEmpty().WithMessage("itemGroup name is required.");
        }
    }
}
