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

namespace TN.Reports.Application.StockDetailReport.Queries
{
    public  class GetStockDetailReportQueryHandler:IRequestHandler<GetStockDetailReportQuery,Result<PagedResult<GetStockDetailReportResponse>>>
    {
        private readonly IStockDetailReportService _service;
        private readonly IMapper _mapper;

        public GetStockDetailReportQueryHandler(IStockDetailReportService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        
        }

        public async Task<Result<PagedResult<GetStockDetailReportResponse>>> Handle(GetStockDetailReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reportResult = await _service.GetStockDetailReport(request.companyId,request.PaginationRequest,request.FilterStockDetailReportDto, cancellationToken);

                if (!reportResult.IsSuccess)
                    return Result<PagedResult<GetStockDetailReportResponse>>.Failure(
                    reportResult.Message ?? "Failed to fetch report");
                var mappedData = _mapper.Map<PagedResult<GetStockDetailReportResponse>>(reportResult.Data);

                return Result<PagedResult<GetStockDetailReportResponse>>.Success(mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching stock detail report", ex);
            }
        }
    }
}
