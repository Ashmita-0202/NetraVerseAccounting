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

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport
{
    public sealed class LedgerBalanceQueryHandler : IRequestHandler<LedgerBalanceReportQueries, Result<PagedResult<LedgerBalanceReportQueryResponse>>>
    {

        private readonly ILedgerBalanceServices _ledgerBalanceServices;
        private readonly IMapper _mapper;

        public LedgerBalanceQueryHandler(ILedgerBalanceServices ledgerBalanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _ledgerBalanceServices = ledgerBalanceServices;
        }
        public async Task<Result<PagedResult<LedgerBalanceReportQueryResponse>>> Handle(LedgerBalanceReportQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _ledgerBalanceServices.GetLedgerBalanceReportByLedger(request.PaginationRequest, request.ledgerBalanceDTOs);

                var ledgerBalanceResult = _mapper.Map<PagedResult<LedgerBalanceReportQueryResponse>>(result.Data);

                return Result<PagedResult<LedgerBalanceReportQueryResponse>>.Success(ledgerBalanceResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<LedgerBalanceReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
