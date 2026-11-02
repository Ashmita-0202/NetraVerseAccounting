using AutoMapper;
using MediatR;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.TrialBalance
{
    public class TrialBalanceQueryHandler : IRequestHandler<TrialBalanceQuery, Result<PagedResult<MasterLevelQueryRespones>>>
    {
        private readonly ITrialBalanceServices _trialBalanceservices;
        private readonly IMapper _mapper;


        public TrialBalanceQueryHandler(ITrialBalanceServices trialBalanceServices, IMapper mapper)
        {
            _trialBalanceservices = trialBalanceServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<MasterLevelQueryRespones>>> Handle(TrialBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alltrialBalanceReport = await _trialBalanceservices.GetTrialBalanceReport(request.PaginationRequest,request.companyId);
                var alltrialBalanceReportDisplay = _mapper.Map<PagedResult<MasterLevelQueryRespones>>(alltrialBalanceReport.Data);

                return Result<PagedResult<MasterLevelQueryRespones>>.Success(alltrialBalanceReportDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchase Items", ex);
            }
        }
    }
}
