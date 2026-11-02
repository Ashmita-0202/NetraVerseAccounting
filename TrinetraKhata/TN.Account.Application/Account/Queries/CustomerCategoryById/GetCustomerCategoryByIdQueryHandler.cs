using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.CustomerCategoryById
{
    public sealed class GetCustomerCategoryByIdQueryHandler:IRequestHandler<GetCustomerCategoryByIdQuery, Result<GetCustomerCategoryByIdResponse>>
    {
        private readonly ICustomerCategoryService _customerCategoryService;
        private readonly IMapper _mapper;

        public GetCustomerCategoryByIdQueryHandler(ICustomerCategoryService customerCategoryService,IMapper mapper)
        { 
            _customerCategoryService=customerCategoryService;
            _mapper=mapper;
        
        }

        public async Task<Result<GetCustomerCategoryByIdResponse>> Handle(GetCustomerCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var customerCategoryById = await _customerCategoryService.GetCustomerCategoryById(request.id);
                return Result<GetCustomerCategoryByIdResponse>.Success(customerCategoryById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching ledger by using id", ex);
            }
        }
    }
}
