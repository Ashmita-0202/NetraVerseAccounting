using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TN.Sales.Application.Sales.Command.QuotationToSales
{
    public class QuotationToSalesCommandHandler : IRequestHandler<QuotationToSalesCommand, Result<QuotationToSalesResponse>>
    {

        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IValidator<QuotationToSalesCommand> _validator;
        private readonly IMapper _mapper;

        public QuotationToSalesCommandHandler(ISalesDetailsServices salesDetailsServices, IValidator<QuotationToSalesCommand> validator, IMapper mapper)
        {
            _validator = validator;
            _mapper = mapper;
            _salesDetailsServices = salesDetailsServices;

        }
        public async Task<Result<QuotationToSalesResponse>> Handle(QuotationToSalesCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result<QuotationToSalesResponse>.Failure(errors);
            }

            var quotationToSales = await _salesDetailsServices.QuotationToSales(request);
            return Result<QuotationToSalesResponse>.Success(quotationToSales.Data);
        }
    }
    
}
