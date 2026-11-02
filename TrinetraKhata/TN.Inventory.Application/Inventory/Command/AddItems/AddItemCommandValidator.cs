
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.AddItems
{
  public class AddItemCommandValidator: AbstractValidator<AddItemCommand>
    {
        public AddItemCommandValidator()
        {
            RuleFor(x => x.name)
                 .NotEmpty().WithMessage("item name is required.");
        }
  }
    
}
