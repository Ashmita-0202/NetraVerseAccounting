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
namespace TN.Sales.Application.Sales.Command.UpdateSalesDetails
{
    public class UpdateSalesDetailsCommandHandler : IRequestHandler<UpdateSalesDetailsCommand, Result<UpdateSalesDetailsResponse>>
    {

        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSalesDetailsCommand> _validator;

        public UpdateSalesDetailsCommandHandler(ISalesDetailsServices SalesDetailsServices, IMapper mapper, IValidator<UpdateSalesDetailsCommand> validator )
        {
            _salesDetailsServices = SalesDetailsServices;
            _mapper = mapper;
            _validator = validator;


        }
        public async Task<Result<UpdateSalesDetailsResponse>> Handle(UpdateSalesDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSalesDetailsResponse>.Failure(errors);

                }

                var updateSalesDetails = await _salesDetailsServices.Update(request.id, request);

                if (updateSalesDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updateSalesDetails.Errors);
                    return Result<UpdateSalesDetailsResponse>.Failure(errors);
                }

                if (updateSalesDetails is null || !updateSalesDetails.IsSuccess)
                {
                    return Result<UpdateSalesDetailsResponse>.Failure("Updates purchase details failed");
                }

                var purchaseDisplay = _mapper.Map<UpdateSalesDetailsResponse>(updateSalesDetails.Data);
                return Result<UpdateSalesDetailsResponse>.Success(purchaseDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating sales details by {request.id}", ex);
            }
        }
    }
}
