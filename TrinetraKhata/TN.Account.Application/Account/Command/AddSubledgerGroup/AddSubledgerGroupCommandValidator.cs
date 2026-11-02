using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.AddSubledgerGroup
{
    public  class AddSubledgerGroupCommandValidator:AbstractValidator<AddSubledgerGroupCommand>
    {
        public AddSubledgerGroupCommandValidator()
        {
            RuleFor(x => x.ledgerGroupId)
                .NotEmpty().WithMessage("ledgerGroupId is required.");
                
        }
    }
}
