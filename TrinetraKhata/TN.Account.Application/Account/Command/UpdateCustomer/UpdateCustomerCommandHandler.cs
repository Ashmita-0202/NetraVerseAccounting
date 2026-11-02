using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateCustomer
{
    public class UpdateCustomerCommandHandler: IRequestHandler<UpdateCustomerCommand, Result<UpdateCustomerResponse>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCustomerCommand> _validator;

        public UpdateCustomerCommandHandler(ICustomerService customerService, IMapper mapper,IValidator<UpdateCustomerCommand> validator) 
        { 
            _customerService=customerService;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<UpdateCustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateCustomerResponse>.Failure(errors);

                }

                var updateCustomer = await _customerService.Update(request.id, request);

                if (updateCustomer.Errors.Any())
                {
                    var errors = string.Join(", ", updateCustomer.Errors);
                    return Result<UpdateCustomerResponse>.Failure(errors);
                }

                if (updateCustomer is null || !updateCustomer.IsSuccess)
                {
                    return Result<UpdateCustomerResponse>.Failure("Updates customer failed");
                }

                var customerDisplay = _mapper.Map<UpdateCustomerResponse>(updateCustomer.Data);
                return Result<UpdateCustomerResponse>.Success(customerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating customer by {request.id}", ex);
            }
        }
    }
}
