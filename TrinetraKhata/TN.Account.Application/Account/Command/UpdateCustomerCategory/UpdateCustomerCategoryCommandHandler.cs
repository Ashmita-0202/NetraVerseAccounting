using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateCustomerCategory
{
    public  class UpdateCustomerCategoryCommandHandler:IRequestHandler<UpdateCustomerCategoryCommand, Result<UpdateCustomerCategoryResponse>>
    {
        private readonly ICustomerCategoryService _customerCategoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCustomerCategoryCommand> _validator;

        public UpdateCustomerCategoryCommandHandler(ICustomerCategoryService customerCategoryService,IMapper mapper,IValidator<UpdateCustomerCategoryCommand> validator)
        { 
            _customerCategoryService= customerCategoryService;
            _mapper= mapper;
            _validator= validator;
        }

        public async Task<Result<UpdateCustomerCategoryResponse>> Handle(UpdateCustomerCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateCustomerCategoryResponse>.Failure(errors);

                }

                var updateCustomerCategory = await _customerCategoryService.Update(request.id, request);

                if (updateCustomerCategory.Errors.Any())
                {
                    var errors = string.Join(", ", updateCustomerCategory.Errors);
                    return Result<UpdateCustomerCategoryResponse>.Failure(errors);
                }

                if (updateCustomerCategory is null || !updateCustomerCategory.IsSuccess)
                {
                    return Result<UpdateCustomerCategoryResponse>.Failure("Updates customer failed");
                }

                var customerDisplay = _mapper.Map<UpdateCustomerCategoryResponse>(updateCustomerCategory.Data);
                return Result<UpdateCustomerCategoryResponse>.Success(customerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating customer by {request.id}", ex);
            }
        }
    }
}
