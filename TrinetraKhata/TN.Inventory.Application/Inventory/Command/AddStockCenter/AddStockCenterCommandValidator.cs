using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.AddStockCenter
{
    public  class AddStockCenterCommandValidator:AbstractValidator<AddStockCenterCommand>
    {
        public AddStockCenterCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stock Center Name is required.");
                
        }
    }
}
