using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails
{
    public class AddPurchaseReturnDetailsCommandHandler : IRequestHandler<AddPurchaseReturnDetailsCommand, Result<AddPurchaseReturnDetailsResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPurchaseReturnDetailsCommand> _validator;


        public AddPurchaseReturnDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper, IValidator<AddPurchaseReturnDetailsCommand> validator)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddPurchaseReturnDetailsResponse>> Handle(AddPurchaseReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPurchaseReturnDetailsResponse>.Failure(errors);
                }

                var addPurchaseReturnDetails = await _purchaseDetailsServices.AddPurchaseReturn(request);

                if (addPurchaseReturnDetails.Errors.Any())
                {
                    var errors = string.Join(", ", addPurchaseReturnDetails.Errors);
                    return Result<AddPurchaseReturnDetailsResponse>.Failure(errors);
                }

                if (addPurchaseReturnDetails is null || !addPurchaseReturnDetails.IsSuccess)
                {
                    return Result<AddPurchaseReturnDetailsResponse>.Failure(" ");
                }

                var purchaseReturnDetails = _mapper.Map<AddPurchaseReturnDetailsResponse>(addPurchaseReturnDetails.Data);
                return Result<AddPurchaseReturnDetailsResponse>.Success(purchaseReturnDetails);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding purchaseReturn", ex);


            }
        }
    }
}
