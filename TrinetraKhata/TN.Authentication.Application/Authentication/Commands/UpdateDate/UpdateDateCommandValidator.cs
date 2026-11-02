using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Authentication.Application.Authentication.Commands.UpdateDate
{
    public  class UpdateDateCommandValidator:AbstractValidator<UpdateDateCommand>
    {
        public UpdateDateCommandValidator()
        {
            RuleFor(x => x.Date)
                         .NotEmpty()
                         .WithMessage("date is required.");
        }
    }
}
