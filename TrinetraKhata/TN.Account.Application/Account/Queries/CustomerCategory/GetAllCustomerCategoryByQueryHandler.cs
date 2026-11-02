using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.CustomerCategory
{
    public sealed class GetAllCustomerCategoryByQueryHandler:IRequestHandler<GetAllCustomerCategoryByQuery,Result<PagedResult<GetAllCustomerCategoryByResponse>>>
    {
        private readonly ICustomerCategoryService _customerCategoryService;
        private readonly IMapper _mapper;

        public GetAllCustomerCategoryByQueryHandler(ICustomerCategoryService customerCategoryService,IMapper mapper)
        {
            _customerCategoryService= customerCategoryService;
            _mapper= mapper;

        }

        public async Task<Result<PagedResult<GetAllCustomerCategoryByResponse>>> Handle(GetAllCustomerCategoryByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCustomerCategory = await _customerCategoryService.GetAllCustomerCategory(request.PaginationRequest, cancellationToken);
                var allCustomerCategoryDisplay = _mapper.Map<PagedResult<GetAllCustomerCategoryByResponse>>(allCustomerCategory.Data);
                return Result<PagedResult<GetAllCustomerCategoryByResponse>>.Success(allCustomerCategoryDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer category", ex);
            }
        }
    }
}
