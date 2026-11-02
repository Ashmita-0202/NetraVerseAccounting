using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails
{
    public  class UpdateStockTransferDetailsCommandValidator:AbstractValidator<UpdateStockTransferDetailsCommand>
    {
        public UpdateStockTransferDetailsCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
        }
    }
}
