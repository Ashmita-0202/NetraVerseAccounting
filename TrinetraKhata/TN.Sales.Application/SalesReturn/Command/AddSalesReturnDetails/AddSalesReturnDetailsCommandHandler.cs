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

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails
{
    public class AddSalesReturnDetailsCommandHandler : IRequestHandler<AddSalesReturnDetailsCommand, Result<AddSalesReturnDetailsResponse>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSalesReturnDetailsCommand> _validator;

        public AddSalesReturnDetailsCommandHandler(ISalesReturnServices salesReturnServices,IMapper mapper,IValidator<AddSalesReturnDetailsCommand> validator)
        {
            _salesReturnServices=salesReturnServices;
            _mapper=mapper;
            _validator=validator;

        }

        public async Task<Result<AddSalesReturnDetailsResponse>> Handle(AddSalesReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSalesReturnDetailsResponse>.Failure(errors);
                }

                var addSalesReturn = await _salesReturnServices.Add(request);

                if (addSalesReturn.Errors.Any())
                {
                    var errors = string.Join(", ", addSalesReturn.Errors);
                    return Result<AddSalesReturnDetailsResponse>.Failure(errors);
                }

                if (addSalesReturn is null || !addSalesReturn.IsSuccess)
                {
                    return Result<AddSalesReturnDetailsResponse>.Failure(" ");
                }

                var salesReturnDisplay = _mapper.Map<AddSalesReturnDetailsResponse>(addSalesReturn.Data);
                return Result<AddSalesReturnDetailsResponse>.Success(salesReturnDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding sales return Details", ex);


            }
        }
    }
}
