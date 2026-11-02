using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterLedgerGroup
{
    public class GetFilterLedgerGroupQueryHandler:IRequestHandler<GetFilterLedgerGroupQuery, Result<PagedResult<GetFilterLedgerGroupQueryResponse>>>
    {
        private readonly ILedgerGroupService _service;
        private readonly IMapper _mapper;

        public GetFilterLedgerGroupQueryHandler(ILedgerGroupService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetFilterLedgerGroupQueryResponse>>> Handle(GetFilterLedgerGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _service.GetFilterLedgerGroup(request.PaginationRequest, request.FilterLedgerGroupDto);

                var ledgerGroupResult = _mapper.Map<PagedResult<GetFilterLedgerGroupQueryResponse>>(result.Data);

                return Result<PagedResult<GetFilterLedgerGroupQueryResponse>>.Success(ledgerGroupResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterLedgerGroupQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
