using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockCenter
{
    public class UpdateStockCenterCommandValidator:AbstractValidator<UpdateStockCenterCommand>
    {
        public UpdateStockCenterCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
        }
    }
}
