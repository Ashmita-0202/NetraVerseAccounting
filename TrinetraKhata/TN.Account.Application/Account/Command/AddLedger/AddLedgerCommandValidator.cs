using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TN.Account.Application.Account.Command.AddLedgerGroup;

namespace TN.Account.Application.Account.Command.AddLedger
{
    public class AddLedgerCommandValidator: AbstractValidator<AddLedgerCommand>
    {

        public AddLedgerCommandValidator()
        {
            RuleFor(x => x.name)
               .NotEmpty()
               .WithMessage("Name is required.")
               .MaximumLength(100)
               .WithMessage("Name cannot exceed 100 characters.");


            //RuleFor(x => x.nepaliDate)
            //    .NotEmpty()
            //    .WithMessage("Nepali Date is required.")
            //    .Matches(@"\d{4}-\d{2}-\d{2}")
            //    .WithMessage("Nepali Date must be in the format YYYY-MM-DD.");


            //RuleFor(x => x.isInventoryAffected)
            //    .NotNull()
            //    .WithMessage("Inventory Affected flag is required.");

            
            //RuleFor(x => x.address)
            //    .MaximumLength(200)
            //    .WithMessage("Address cannot exceed 200 characters.");

            
            //RuleFor(x => x.panNo)
            //    .NotEmpty()
            //    .WithMessage("PAN Number is required.")
            //    //.Matches(@"^[A-Za-z0-9]{5,10}$")
            //    .WithMessage("PAN Number must be alphanumeric and between 5 to 10 characters.");


        }

    }
}
