using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;


namespace TN.Inventory.Application.Inventory.Command.UpdateItem
{
    public class UpdateItemCommandValidator:AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        { 
            RuleFor(x => x.name)
                 .NotEmpty().WithMessage("item name is required.");

            //RuleFor(x => x.price)
            //     .NotEmpty().WithMessage("price is required.");

            //RuleFor(x => x.sellingPrice)
            //     .NotEmpty().WithMessage("sellingPrice is required.");

            //RuleFor(x => x.costPrice)
            //     .NotEmpty().WithMessage("costPrice is required.");

            //RuleFor(x => x.barCodeField)
            //     .NotEmpty().WithMessage("barCodeField is required.");

        }
    }
}
