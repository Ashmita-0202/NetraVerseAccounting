using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteCustomerCategory
{
    public class DeleteCustomerCategoryCommandHandler:IRequestHandler<DeleteCustomerCategoryCommand,Result<bool>>
    {
        private readonly ICustomerCategoryService _customerCategoryService;
        private readonly IMapper _mapper;

        public DeleteCustomerCategoryCommandHandler(ICustomerCategoryService customerCategoryService,IMapper mapper)
        {
            _customerCategoryService=customerCategoryService;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteCustomerCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteCustomerCategory = await _customerCategoryService.Delete(request.id, cancellationToken);
                if (deleteCustomerCategory is null)
                {
                    return Result<bool>.Failure("NotFound", "Ledger not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
