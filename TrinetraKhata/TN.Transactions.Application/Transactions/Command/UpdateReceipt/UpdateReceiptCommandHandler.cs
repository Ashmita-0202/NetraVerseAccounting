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
using TN.Transactions.Application.Transactions.Command.UpdateTransactions;

namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt
{
    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, Result<UpdateReceiptResponse>> 
    {
        private readonly IReceiptServices _receiptServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateReceiptCommand> _validator;

        public UpdateReceiptCommandHandler(IReceiptServices receiptServices,IMapper mapper, IValidator<UpdateReceiptCommand> validator)
        {
            _receiptServices=receiptServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateReceiptResponse>> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateReceiptResponse>.Failure(errors);

                }

                var updateReceipt = await _receiptServices.Update(request, request.id);

                if (updateReceipt.Errors.Any())
                {
                    var errors = string.Join(", ", updateReceipt.Errors);
                    return Result<UpdateReceiptResponse>.Failure(errors);
                }

                if (updateReceipt is null || !updateReceipt.IsSuccess)
                {
                    return Result<UpdateReceiptResponse>.Failure("Updates Transactions failed");
                }

                var receiptDisplay = _mapper.Map<UpdateReceiptResponse>(updateReceipt.Data);
                return Result<UpdateReceiptResponse>.Success(receiptDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating receipt by {request.id}", ex);
            }
        }
    }
    
}
