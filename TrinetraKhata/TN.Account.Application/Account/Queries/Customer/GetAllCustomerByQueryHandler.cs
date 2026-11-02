using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.Customer
{
    public sealed class GetAllCustomerByQueryHandler:IRequestHandler<GetAllCustomerByQuery,Result<PagedResult<GetAllCustomerByQueryResponse>>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public GetAllCustomerByQueryHandler(ICustomerService customerService, IMapper mapper) 
        { 
            _customerService=customerService;
            _mapper=mapper;
        
        }

        public async Task<Result<PagedResult<GetAllCustomerByQueryResponse>>> Handle(GetAllCustomerByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCustomer = await _customerService.GetAllCustomer(request.PaginationRequest, cancellationToken);
                var allCustomerDisplay = _mapper.Map<PagedResult<GetAllCustomerByQueryResponse>>(allCustomer.Data);
                return Result<PagedResult<GetAllCustomerByQueryResponse>>.Success(allCustomerDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
