using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails
{
    public class UpdateSalesReturnDetailsCommandHandler : IRequestHandler<UpdateSalesReturnDetailsCommand, Result<UpdateSalesReturnDetailsResponse>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSalesReturnDetailsCommand> _validator;

        public UpdateSalesReturnDetailsCommandHandler(ISalesReturnServices salesReturnServices,IMapper mapper,IValidator<UpdateSalesReturnDetailsCommand> validator)
        {
            _salesReturnServices=salesReturnServices;
            _mapper=mapper;
            _validator=validator;
            
        }

        public async Task<Result<UpdateSalesReturnDetailsResponse>> Handle(UpdateSalesReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSalesReturnDetailsResponse>.Failure(errors);

                }

                var updateSalesReturnDetails = await _salesReturnServices.Update(request,request.id);

                if (updateSalesReturnDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updateSalesReturnDetails.Errors);
                    return Result<UpdateSalesReturnDetailsResponse>.Failure(errors);
                }

                if (updateSalesReturnDetails is null || !updateSalesReturnDetails.IsSuccess)
                {
                    return Result<UpdateSalesReturnDetailsResponse>.Failure("Updates Sales Return details failed");
                }

                var journalDetailsDisplay = _mapper.Map<UpdateSalesReturnDetailsResponse>(updateSalesReturnDetails.Data);
                return Result<UpdateSalesReturnDetailsResponse>.Success(journalDetailsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Sales Return  details by {request.id}", ex);
            }
        }
    }
    
}
