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

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public class AddPurchaseItemsCommandHandler : IRequestHandler<AddPurchaseItemsCommand, Result<AddPurchaseItemsResponse>>
    {
        private readonly IPurchaseItemsServices _purchaseItemsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPurchaseItemsCommand> _validator;

        public AddPurchaseItemsCommandHandler(IPurchaseItemsServices purchaseItemsServices, IMapper mapper, IValidator<AddPurchaseItemsCommand> validator)
        {
            _purchaseItemsServices = purchaseItemsServices;
            _mapper = mapper;
            _validator = validator;

        }

        public async Task<Result<AddPurchaseItemsResponse>> Handle(AddPurchaseItemsCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPurchaseItemsResponse>.Failure(errors);
                }

                var addPurchaseItems = await _purchaseItemsServices.Add(request);

                if (addPurchaseItems.Errors.Any())
                {
                    var errors = string.Join(", ", addPurchaseItems.Errors);
                    return Result<AddPurchaseItemsResponse>.Failure(errors);
                }

                if (addPurchaseItems is null || !addPurchaseItems.IsSuccess)
                {
                    return Result<AddPurchaseItemsResponse>.Failure(" ");
                }

                var customerDisplays = _mapper.Map<AddPurchaseItemsResponse>(addPurchaseItems.Data);
                return Result<AddPurchaseItemsResponse>.Success(customerDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding PurchaseItems", ex);


            }
        }
    }
}