using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType
{
    public  class UpdateExpenseTransactionNumberTypeCommandHandler:IRequestHandler<UpdateExpenseTransactionNumberTypeCommand,Result<UpdateExpenseTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateExpenseTransactionNumberTypeCommand> _validator;

        public UpdateExpenseTransactionNumberTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateExpenseTransactionNumberTypeCommand> validator )
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateExpenseTransactionNumberTypeResponse>> Handle(UpdateExpenseTransactionNumberTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<UpdateExpenseTransactionNumberTypeResponse>.Failure(errors);
            }

            var expenseTransactionType = await _settingServices.UpdateExpenseTransactionType(request.companyId, request.transactionNumberType, cancellationToken);

            if (expenseTransactionType is not { IsSuccess: true, Data: not null })
                return Result<UpdateExpenseTransactionNumberTypeResponse>.Failure(expenseTransactionType?.Message ?? "Unable to update TransactionNumberType reference.");

            return Result<UpdateExpenseTransactionNumberTypeResponse>.Success(
                _mapper.Map<UpdateExpenseTransactionNumberTypeResponse>(expenseTransactionType.Data)
            );
        }
    }
}
