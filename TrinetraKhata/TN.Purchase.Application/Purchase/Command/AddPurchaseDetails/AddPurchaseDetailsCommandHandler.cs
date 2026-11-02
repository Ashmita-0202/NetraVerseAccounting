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

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public class AddPurchaseDetailsCommandHandler : IRequestHandler<AddPurchaseDetailsCommand, Result<AddPurchaseDetailsResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPurchaseDetailsCommand> _validator;

        public AddPurchaseDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper, IValidator<AddPurchaseDetailsCommand> validator)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<Result<AddPurchaseDetailsResponse>> Handle(AddPurchaseDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPurchaseDetailsResponse>.Failure(errors);
                }

                var addPurchaseDetails = await _purchaseDetailsServices.Add(request);

                if (addPurchaseDetails.Errors.Any())
                {
                    var errors = string.Join(", ", addPurchaseDetails.Errors);
                    return Result<AddPurchaseDetailsResponse>.Failure(errors);
                }

                if (addPurchaseDetails is null || !addPurchaseDetails.IsSuccess)
                {
                    return Result<AddPurchaseDetailsResponse>.Failure(" ");
                }
                var addPurchaseDetailsResponse = _mapper.Map<AddPurchaseDetailsResponse>(addPurchaseDetails.Data);
                return Result<AddPurchaseDetailsResponse>.Success(addPurchaseDetailsResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding purchaseDetails", ex);
            }
        }
    }
}