using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateMaster
{
   public  class UpdateMasterCommandValidator:AbstractValidator<UpdateMasterCommand>
    {
        public UpdateMasterCommandValidator()
        {
           

            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Ledger name is required.");


        }
    }
}
