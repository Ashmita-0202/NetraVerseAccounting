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

namespace TN.Reports.Application.Annex13.Queries
{
    public class AnnexReportQueryHandler: IRequestHandler<AnnexReportQuery , Result<PagedResult<AnnexReportQueryResponse>>>
    {
        private readonly IAnnexReportServices _annexReportServices;
        private readonly IMapper _mapper;

        public AnnexReportQueryHandler(IAnnexReportServices annexReportServices, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _annexReportServices = annexReportServices ?? throw new ArgumentNullException(nameof(annexReportServices));

        }
        public async Task<Result<PagedResult<AnnexReportQueryResponse>>> Handle(AnnexReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allAnnexReport = await _annexReportServices.GetAnnexReport(request.PaginationRequest, request.AnnexReportDTOs, cancellationToken);

                var allAnnexReportDisplay = _mapper.Map<PagedResult<AnnexReportQueryResponse>>(allAnnexReport.Data);

                return Result<PagedResult<AnnexReportQueryResponse>>.Success(allAnnexReportDisplay);

            }
            catch(Exception ex)
            {
                return Result<PagedResult<AnnexReportQueryResponse>>.Failure(ex.Message);
            }
        }
    }
   
}
