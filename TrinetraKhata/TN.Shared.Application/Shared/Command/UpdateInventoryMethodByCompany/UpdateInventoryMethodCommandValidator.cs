using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany
{
    public  class UpdateInventoryMethodCommandValidator:AbstractValidator<UpdateInventoryMethodCommand>
    {
        public UpdateInventoryMethodCommandValidator()
        {
            RuleFor(x => x.inventoryMethod).NotEmpty().WithMessage("inventory method is required");
            
        }
    }
}
