using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.CustomerById
{
    public sealed class GetCustomerByIdQueryHandler:IRequestHandler<GetCustomerByIdQuery,Result<GetCustomerByIdResponse>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerService customerService, IMapper mapper)
        {
            _customerService=customerService;
            _mapper=mapper;
        
        }

        public async Task<Result<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var customerById= await _customerService.GetCustomerById(request.id);
                return Result<GetCustomerByIdResponse>.Success(customerById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching ledger by using id", ex);
            }
        }
    }


}
