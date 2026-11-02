using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.Sales.Command.AddSalesDetails
{
    public class AddSalesDetailsCommandHandler : IRequestHandler<AddSalesDetailsCommand, Result<AddSalesDetailsResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSalesDetailsCommand> _validator;

        public AddSalesDetailsCommandHandler(ISalesDetailsServices SalesDetailsServices, IMapper mapper, IValidator<AddSalesDetailsCommand> validator)
        {
            _salesDetailsServices = SalesDetailsServices;
            _mapper = mapper;
            _validator = validator;
            
        }
        public async Task<Result<AddSalesDetailsResponse>> Handle(AddSalesDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSalesDetailsResponse>.Failure(errors);
                }

                var addSalesDetails = await _salesDetailsServices.Add(request);

                if (addSalesDetails.Errors.Any())
                {
                    var errors = string.Join(", ", addSalesDetails.Errors);
                    return Result<AddSalesDetailsResponse>.Failure(errors);
                }

                if (addSalesDetails is null || !addSalesDetails.IsSuccess)
                {
                    return Result<AddSalesDetailsResponse>.Failure(" ");
                }
                var addSalesDetailsRespones = _mapper.Map<AddSalesDetailsResponse>(addSalesDetails.Data);
                return Result<AddSalesDetailsResponse>.Success(addSalesDetailsRespones);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
