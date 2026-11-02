using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.ItemwiseProfitReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ItemwiseSalesReport
{
    public class ItemwiseSalesReportQueryHandler:IRequestHandler<ItemwiseSalesReportQuery,Result<PagedResult<ItemwiseSalesReportQueryResponse>>>
    {
        private readonly ISalesReportService _salesReportService;
        private readonly IMapper _mapper;

        public ItemwiseSalesReportQueryHandler(ISalesReportService salesReportService,IMapper mapper)
        {
            _salesReportService=salesReportService;
            _mapper= mapper;
        }

        public async Task<Result<PagedResult<ItemwiseSalesReportQueryResponse>>> Handle(ItemwiseSalesReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _salesReportService.GetItemwiseSalesReport(request.PaginationRequest, request.ItemwiseSalesReportDto);
                return Result<PagedResult<ItemwiseSalesReportQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ItemwiseSalesReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
