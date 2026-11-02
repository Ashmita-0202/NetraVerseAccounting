using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup
{
    public class UpdateSubledgerGroupCommandValidator:AbstractValidator<UpdateSubledgerGroupCommand>
    {
        public UpdateSubledgerGroupCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("id is required");
        }
    }
}
