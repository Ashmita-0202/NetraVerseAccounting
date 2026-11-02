using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public class UpdatePaymentTransactionNumberTypeCommandHandler : IRequestHandler<UpdatePaymentTransactionNumberTypeCommand, Result<UpdatePaymentTransactionNumberTypeResponse>>
    {

        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePaymentTransactionNumberTypeCommand> _validator;

        public UpdatePaymentTransactionNumberTypeCommandHandler(ISettingServices settingServices, IMapper mapper, IValidator<UpdatePaymentTransactionNumberTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;

        }


        public async Task<Result<UpdatePaymentTransactionNumberTypeResponse>> Handle(UpdatePaymentTransactionNumberTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<UpdatePaymentTransactionNumberTypeResponse>.Failure(errors);
            }

            var paymentTransactionType = await _settingServices.UpdatePaymentTransactionType(request.companyId, request.transactionNumberType, cancellationToken);

            if (paymentTransactionType is not { IsSuccess: true, Data: not null })
                return Result<UpdatePaymentTransactionNumberTypeResponse>.Failure(paymentTransactionType?.Message ?? "Unable to update TransactionNumberType reference.");

            return Result<UpdatePaymentTransactionNumberTypeResponse>.Success(
                _mapper.Map<UpdatePaymentTransactionNumberTypeResponse>(paymentTransactionType.Data)
            );
        }
    }
}
