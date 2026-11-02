using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TradingAccount;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Profit_LossReport
{
    public class GetProfitLossResporQueryHandler : IRequestHandler<GetProfitLossReportQuery, Result<PagedResult<ProfitAndLossFinalResponse>>>
    {
        private readonly IProfitAndLossServices _services;
        private readonly ITradingServices _tradingServices;
        private readonly IMapper _mapper;

        public GetProfitLossResporQueryHandler(IProfitAndLossServices services, IMapper mapper)
        {
            _services = services;
        
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<ProfitAndLossFinalResponse>>> Handle(GetProfitLossReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var profitandLossReport = await _services.GetProfitLossReport(request.PaginationRequest, request.requestedCompanyId, cancellationToken);
                var profitandLossReportDisplay = _mapper.Map<PagedResult<ProfitAndLossFinalResponse>>(profitandLossReport.Data);

                return Result<PagedResult<ProfitAndLossFinalResponse>>.Success(profitandLossReportDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all Profit and Loss report", ex);
            }
        }
    }
}
