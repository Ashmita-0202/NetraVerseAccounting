using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.SalesRegister
{
    public sealed class SalesRegisterQueryHandler : IRequestHandler<SalesRegisterQueries, Result<PagedResult<SalesRegisterQueryResponse>>>
    {
       private readonly IAccountBookServices _accountBookServices;
        private readonly IMapper _mapper;

        public SalesRegisterQueryHandler(IAccountBookServices accountBookServices, IMapper mapper)
        {
            _accountBookServices = accountBookServices;
            _mapper = mapper;
            
        }

        public async Task<Result<PagedResult<SalesRegisterQueryResponse>>> Handle(SalesRegisterQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _accountBookServices.GetSalesRegister(request.PaginationRequest, request.SalesRegisterDTOs);

                var salesRegisterResponse = _mapper.Map<PagedResult<SalesRegisterQueryResponse>>(result.Data);

                return Result<PagedResult<SalesRegisterQueryResponse>>.Success(salesRegisterResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<SalesRegisterQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
