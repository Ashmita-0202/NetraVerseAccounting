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

namespace TN.Reports.Application.SalesReport
{
    public class GetSalesReportQueryHandler:IRequestHandler<GetSalesReportQuery,Result<PagedResult<GetSalesReportQueryResponse>>>
    {
        private readonly ISalesReportService _service;
        private readonly IMapper _mapper;

        public GetSalesReportQueryHandler(ISalesReportService service,IMapper mapper) 
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetSalesReportQueryResponse>>> Handle(GetSalesReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetAllSalesReport(request.paginationRequest, request.salesReportDtos);
                return Result<PagedResult<GetSalesReportQueryResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetSalesReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
