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

namespace TN.Reports.Application.StockReport.Queries
{
    public class GetStockReportQueryHandler:IRequestHandler<GetStockReportQuery, Result<PagedResult<GetStockReportQueryResponse>>>
    {
        private readonly IStockDetailReportService _stockDetailReportService;
        private readonly IMapper _mapper;

        public GetStockReportQueryHandler(IStockDetailReportService stockDetailReportService,IMapper mapper) 
        {
            _stockDetailReportService = stockDetailReportService;

            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetStockReportQueryResponse>>> Handle(GetStockReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reportResult = await _stockDetailReportService.GetStockReport(request.PaginationRequest,request.StockReportDto, cancellationToken);

                if (!reportResult.IsSuccess)
                    return Result<PagedResult<GetStockReportQueryResponse>>.Failure(
                    reportResult.Message ?? "Failed to fetch report");
                var mappedData = _mapper.Map<PagedResult<GetStockReportQueryResponse>>(reportResult.Data);

                return Result<PagedResult<GetStockReportQueryResponse>>.Success(mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching stock report", ex);
            }
        }
    }
}
