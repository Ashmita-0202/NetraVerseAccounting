using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterParties
{
    public  class GetFilterPartiesQueryHandler:IRequestHandler<GetFilterPartiesQuery,Result<PagedResult<GetFilterPartiesQueryResponse>>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public GetFilterPartiesQueryHandler(ILedgerService ledgerService,IMapper mapper) 
        {
            _ledgerService= ledgerService;
            _mapper= mapper;
        
        }

        public async Task<Result<PagedResult<GetFilterPartiesQueryResponse>>> Handle(GetFilterPartiesQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _ledgerService.GetFilterParties(request.PaginationRequest, request.FilterPartiesDto);

                var partiesResult = _mapper.Map<PagedResult<GetFilterPartiesQueryResponse>>(result.Data);

                return Result<PagedResult<GetFilterPartiesQueryResponse>>.Success(partiesResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterPartiesQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
