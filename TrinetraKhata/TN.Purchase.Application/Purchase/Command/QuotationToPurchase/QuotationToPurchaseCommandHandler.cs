using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.ServiceInterface;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.QuotationToPurchase
{
    public class QuotationToPurchaseCommandHandler : IRequestHandler<QuotationToPurchaseCommand, Result<QuotationToPurchaseResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IValidator<QuotationToPurchaseCommand> _validator;
        private readonly IMapper _mapper;

        public QuotationToPurchaseCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IValidator<QuotationToPurchaseCommand> validator, IMapper mapper)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<QuotationToPurchaseResponse>> Handle(QuotationToPurchaseCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<QuotationToPurchaseResponse>.Failure(errors);
            }

            var quotationToSales = await _purchaseDetailsServices.QuotationToPurchase(request);
            return Result<QuotationToPurchaseResponse>.Success(quotationToSales.Data);
        }
    }
}
