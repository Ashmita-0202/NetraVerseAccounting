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

namespace TN.Account.Application.Account.Queries.FilterLedgerByDate
{
    public class GetFilterLedgerByQueryHandler:IRequestHandler<GetFilterLedgerByQuery,Result<PagedResult<GetFilterLedgerByResponse>>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public GetFilterLedgerByQueryHandler(ILedgerService ledgerService,IMapper mapper)
        {
            _ledgerService=ledgerService;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetFilterLedgerByResponse>>> Handle(GetFilterLedgerByQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _ledgerService.GetFilterLedger(request.PaginationRequest, request.FilterLedgerDto);

                var ledgerBalanceResult = _mapper.Map<PagedResult<GetFilterLedgerByResponse>>(result.Data);

                return Result<PagedResult<GetFilterLedgerByResponse>>.Success(ledgerBalanceResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterLedgerByResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
