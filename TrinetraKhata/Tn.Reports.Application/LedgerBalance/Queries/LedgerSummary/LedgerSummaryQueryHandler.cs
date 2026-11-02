using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerSummary
{
    public sealed class LedgerSummaryQueryHandler : IRequestHandler<LedgerSummaryQueries, Result<PagedResult<LedgerSummaryResponse>>>

    {

        private readonly ILedgerBalanceServices _ledgerBalanceServices;
        private readonly IMapper _mapper;

        public LedgerSummaryQueryHandler(ILedgerBalanceServices ledgerBalanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _ledgerBalanceServices = ledgerBalanceServices;

        }
        public async Task<Result<PagedResult<LedgerSummaryResponse>>> Handle(LedgerSummaryQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _ledgerBalanceServices.GetLedgerSummaryByLedger(request.PaginationRequest, request.ledgerId);

                var ledgerSummaryResult = _mapper.Map<PagedResult<LedgerSummaryResponse>>(result.Data);

                return Result<PagedResult<LedgerSummaryResponse>>.Success(ledgerSummaryResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<LedgerSummaryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
