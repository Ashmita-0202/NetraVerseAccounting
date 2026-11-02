using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateCustomerCategory
{
   public class UpdateCustomerCategoryCommandValidator:AbstractValidator<UpdateCustomerCategoryCommand>
    {
        public UpdateCustomerCategoryCommandValidator() 
        {
            RuleFor(x => x.name)
              .NotEmpty().WithMessage("customercategory name is required.");
        }
    }
}
