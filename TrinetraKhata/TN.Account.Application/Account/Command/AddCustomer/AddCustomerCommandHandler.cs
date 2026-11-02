using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddCustomer
{
    public class AddCustomerCommandHandler:IRequestHandler<AddCustomerCommand,Result<AddCustomerResponse>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCustomerCommand> _validator;

        public AddCustomerCommandHandler(ICustomerService customerService,IMapper mapper,IValidator<AddCustomerCommand> validator)
        { 
            _customerService=customerService;
            _mapper=mapper;
            _validator=validator;
        }

        public async  Task<Result<AddCustomerResponse>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCustomerResponse>.Failure(errors);
                }

                var addCustomer = await _customerService.Add(request);

                if (addCustomer.Errors.Any())
                {
                    var errors = string.Join(", ", addCustomer.Errors);
                    return Result<AddCustomerResponse>.Failure(errors);
                }

                if (addCustomer is null || !addCustomer.IsSuccess)
                {
                    return Result<AddCustomerResponse>.Failure(" ");
                }

                var customerDisplays = _mapper.Map<AddCustomerResponse>(request);
                return Result<AddCustomerResponse>.Success(customerDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Customer", ex);


            }
        }
    }
}
