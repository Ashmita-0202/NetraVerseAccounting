using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddCustomerCategory
{
    public class  AddCustomerCategoryCommandHandler: IRequestHandler<AddCustomerCategoryCommand, Result<AddCustomerCategoryResponse>>
    {
        private readonly ICustomerCategoryService _customerCategoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCustomerCategoryCommand> _validator;

        public AddCustomerCategoryCommandHandler(IValidator<AddCustomerCategoryCommand> validator,ICustomerCategoryService customerCategoryService, IMapper mapper)
        {
            _customerCategoryService=customerCategoryService;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<AddCustomerCategoryResponse>> Handle(AddCustomerCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCustomerCategoryResponse>.Failure(errors);
                }

                var addCustomer = await _customerCategoryService.Add(request);

                if (addCustomer.Errors.Any())
                {
                    var errors = string.Join(", ", addCustomer.Errors);
                    return Result<AddCustomerCategoryResponse>.Failure(errors);
                }

                if (addCustomer is null || !addCustomer.IsSuccess)
                {
                    return Result<AddCustomerCategoryResponse>.Failure(" ");
                }

                var customerCategoryDisplays = _mapper.Map<AddCustomerCategoryResponse>(request);
                return Result<AddCustomerCategoryResponse>.Success(customerCategoryDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Customer Category", ex);


            }
        }
    }
}
