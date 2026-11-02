using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SalesSummaryReport
{
    public  class GetSalesSummaryQueryHandler:IRequestHandler<GetSalesSummaryQuery, Result<PagedResult<GetSalesSummaryQueryResponse>>>
    {
        private readonly ISalesReportService _service;
        private readonly IMapper _mapper;

        public GetSalesSummaryQueryHandler(ISalesReportService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetSalesSummaryQueryResponse>>> Handle(GetSalesSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetSalesSummaryReport(request.PaginationRequest, request.SalesSummaryDtos);
                return Result<PagedResult<GetSalesSummaryQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetSalesSummaryQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
