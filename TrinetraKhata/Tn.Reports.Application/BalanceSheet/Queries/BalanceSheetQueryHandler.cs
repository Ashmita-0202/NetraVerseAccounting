using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TrialBalance;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public class BalanceSheetQueryHandler : IRequestHandler<BalanceSheetQuery, Result<PagedResult<BalanceSheetFinalResponse>>>
    {

        private readonly IBalanceSheetServices _balanceSheetServices;
        private readonly IMapper _mapper;

        public BalanceSheetQueryHandler(IBalanceSheetServices balanceSheetServices, IMapper mapper)
        {
            _balanceSheetServices = balanceSheetServices;
            _mapper = mapper;
            
        }

        public async Task<Result<PagedResult<BalanceSheetFinalResponse>>> Handle(BalanceSheetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allBalanceSheetReport = await _balanceSheetServices.GetBalanceSheetReport(request.PaginationRequest, request.CompanyId, cancellationToken);
                var allBalanceSheetReportDisplay = _mapper.Map<PagedResult<BalanceSheetFinalResponse>>(allBalanceSheetReport.Data);

                return Result<PagedResult<BalanceSheetFinalResponse>>.Success(allBalanceSheetReportDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchase Items", ex);
            }
        }
    }
}
