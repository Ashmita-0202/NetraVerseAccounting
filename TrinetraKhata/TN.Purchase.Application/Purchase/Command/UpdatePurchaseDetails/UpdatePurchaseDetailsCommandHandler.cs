using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails
{
    public class UpdatePurchaseDetailsCommandHandler:IRequestHandler<UpdatePurchaseDetailsCommand, Result<UpdatePurchaseDetailsResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePurchaseDetailsCommand> _validator;

        public UpdatePurchaseDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper, IValidator<UpdatePurchaseDetailsCommand> validator) 
        {
        
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdatePurchaseDetailsResponse>> Handle(UpdatePurchaseDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePurchaseDetailsResponse>.Failure(errors);

                }

                var updatePurchaseDetails = await _purchaseDetailsServices.Update(request.id, request);

                if (updatePurchaseDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updatePurchaseDetails.Errors);
                    return Result<UpdatePurchaseDetailsResponse>.Failure(errors);
                }

                if (updatePurchaseDetails is null || !updatePurchaseDetails.IsSuccess)
                {
                    return Result<UpdatePurchaseDetailsResponse>.Failure("Updates purchase details failed");
                }

                var purchaseDisplay = _mapper.Map<UpdatePurchaseDetailsResponse>(updatePurchaseDetails.Data);
                return Result<UpdatePurchaseDetailsResponse>.Success(purchaseDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating purchase details by {request.id}", ex);
            }
        }
    }
}
