using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.AddIncome;

namespace TN.Transactions.Application.Transactions.Command.AddExpense
{
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, Result<AddExpenseResponse>>
    {
        private readonly IExpenseService _expenseServices;
        private readonly IValidator<AddExpenseCommand> _validator;
        private readonly IMapper _mapper;


        public AddExpenseCommandHandler(IExpenseService expenseService, IValidator<AddExpenseCommand> validator)
        {
            _expenseServices = expenseService;
            _validator = validator;
            
        }
        public async Task<Result<AddExpenseResponse>> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddExpenseResponse>.Failure(errors);
                }

                var addIncome = await _expenseServices.AddExpense(request);

                if (addIncome.Errors.Any())
                {
                    var errors = string.Join(", ", addIncome.Errors);
                    return Result<AddExpenseResponse>.Failure(errors);
                }

                if (addIncome is null || !addIncome.IsSuccess)
                {
                    return Result<AddExpenseResponse>.Failure(" ");
                }
                if (addIncome.Data == null)
                {
                    return Result<AddExpenseResponse>.Failure("No income data returned.");
                }


                return Result<AddExpenseResponse>.Success(addIncome.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Transactions", ex);
            }
        }
    }
}
