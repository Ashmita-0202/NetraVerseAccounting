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

namespace TN.Reports.Application.SalesReturn_Report
{
    public  class GetSalesReturnReportQueryHandler: IRequestHandler<GetSalesReturnReportQuery, Result<PagedResult<GetSalesReturnReportQueryResponse>>>
    {
        private readonly ISalesReportService _salesReportService;
        private readonly IMapper _mapper;

        public GetSalesReturnReportQueryHandler(ISalesReportService salesReportService,IMapper mapper )
        {
            _salesReportService = salesReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetSalesReturnReportQueryResponse>>> Handle(GetSalesReturnReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _salesReportService.GetSalesReturnReport(request.PaginationRequest, request.SalesReturnReportDto);
                return Result<PagedResult<GetSalesReturnReportQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetSalesReturnReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
