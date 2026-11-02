using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.AddCustomerCategory
{
    public class AddCustomerCategoryCommandValidator:AbstractValidator<AddCustomerCategoryCommand>
    {
        public AddCustomerCategoryCommandValidator() 
        {
            RuleFor(x => x.name)
              .NotEmpty().WithMessage("customercategory name is required.");

        }
    }
}
