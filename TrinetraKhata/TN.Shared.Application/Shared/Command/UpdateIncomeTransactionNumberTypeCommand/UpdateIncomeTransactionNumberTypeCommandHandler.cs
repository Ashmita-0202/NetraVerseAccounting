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
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand
{
    public  class UpdateIncomeTransactionNumberTypeCommandHandler: IRequestHandler<UpdateIncomeTransactionNumberTypeCommand, Result<UpdateIncomeTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateIncomeTransactionNumberTypeCommand> _validator;

        public UpdateIncomeTransactionNumberTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateIncomeTransactionNumberTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateIncomeTransactionNumberTypeResponse>> Handle(UpdateIncomeTransactionNumberTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<UpdateIncomeTransactionNumberTypeResponse>.Failure(errors);
            }

            var incomeTransactionType = await _settingServices.UpdateIncomeTransactionType(request.companyId, request.transactionNumberType, cancellationToken);

            if (incomeTransactionType is not { IsSuccess: true, Data: not null })
                return Result<UpdateIncomeTransactionNumberTypeResponse>.Failure(incomeTransactionType?.Message ?? "Unable to update TransactionNumberType reference.");

            return Result<UpdateIncomeTransactionNumberTypeResponse>.Success(
                _mapper.Map<UpdateIncomeTransactionNumberTypeResponse>(incomeTransactionType.Data)
            );
        }
    }
}
