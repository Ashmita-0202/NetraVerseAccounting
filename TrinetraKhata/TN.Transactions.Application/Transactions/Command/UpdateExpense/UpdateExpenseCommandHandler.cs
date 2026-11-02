using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense
{
    public class UpdateExpenseCommandHandler: IRequestHandler<UpdateExpenseCommand, Result<UpdateExpenseResponse>>
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateExpenseCommand> _validator;

        public UpdateExpenseCommandHandler(IExpenseService expenseService,IMapper mapper,IValidator<UpdateExpenseCommand> validator)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateExpenseResponse>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateExpenseResponse>.Failure(errors);

                }

                var updateExpense = await _expenseService.UpdateExpense(request.id, request);

                if (updateExpense.Errors.Any())
                {
                    var errors = string.Join(", ", updateExpense.Errors);
                    return Result<UpdateExpenseResponse>.Failure(errors);
                }

                if (updateExpense is null || !updateExpense.IsSuccess)
                {
                    return Result<UpdateExpenseResponse>.Failure("Updates Expense Transactions failed");
                }

                var receiptDisplay = _mapper.Map<UpdateExpenseResponse>(updateExpense.Data);
                return Result<UpdateExpenseResponse>.Success(receiptDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating expense by {request.id}", ex);
            }
        }
    }
}
