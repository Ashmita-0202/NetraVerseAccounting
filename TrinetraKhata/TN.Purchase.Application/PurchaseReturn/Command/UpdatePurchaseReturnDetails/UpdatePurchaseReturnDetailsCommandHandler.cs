using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails
{
    public class UpdatePurchaseReturnDetailsCommandHandler:IRequestHandler<UpdatePurchaseReturnDetailsCommand, Result<UpdatePurchaseReturnDetailsResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePurchaseReturnDetailsCommand> _validator;

        public UpdatePurchaseReturnDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices,IMapper mapper,IValidator<UpdatePurchaseReturnDetailsCommand> validator)
        {
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdatePurchaseReturnDetailsResponse>> Handle(UpdatePurchaseReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePurchaseReturnDetailsResponse>.Failure(errors);

                }

                var updatePurchaseReturnDetails = await _purchaseDetailsServices.Update(request.id, request);

                if (updatePurchaseReturnDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updatePurchaseReturnDetails.Errors);
                    return Result<UpdatePurchaseReturnDetailsResponse>.Failure(errors);
                }

                if (updatePurchaseReturnDetails is null || !updatePurchaseReturnDetails.IsSuccess)
                {
                    return Result<UpdatePurchaseReturnDetailsResponse>.Failure("Updates purchase return details failed");
                }

                var purchaseReturnDetailsDisplay = _mapper.Map<UpdatePurchaseReturnDetailsResponse>(updatePurchaseReturnDetails.Data);
                return Result<UpdatePurchaseReturnDetailsResponse>.Success(purchaseReturnDetailsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating purchase Return details by {request.id}", ex);
            }
        }
    }
}
