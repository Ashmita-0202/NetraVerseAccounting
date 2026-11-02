using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public class UpdateReceiptTransactionNumberTypeCommandHandler : IRequestHandler<UpdateReceiptTransactionNumberTypeCommand, Result<UpdateReceiptTransactionNumberTypeResponse>>
    {

        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateReceiptTransactionNumberTypeCommand> _validator;

        public UpdateReceiptTransactionNumberTypeCommandHandler(ISettingServices settingServices, IMapper mapper, IValidator<UpdateReceiptTransactionNumberTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
            
        }


        public async Task<Result<UpdateReceiptTransactionNumberTypeResponse>> Handle(UpdateReceiptTransactionNumberTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<UpdateReceiptTransactionNumberTypeResponse>.Failure(errors);
            }

            var receiptTransactionType = await _settingServices.UpdateReceiptTransactionType(request.companyId,request.transactionNumberType, cancellationToken);

            if (receiptTransactionType is not { IsSuccess: true, Data: not null })
                return Result<UpdateReceiptTransactionNumberTypeResponse>.Failure(receiptTransactionType?.Message ?? "Unable to update TransactionNumberType reference.");

            return Result<UpdateReceiptTransactionNumberTypeResponse>.Success(
                _mapper.Map<UpdateReceiptTransactionNumberTypeResponse>(receiptTransactionType.Data)
            );
        }
    }
}
