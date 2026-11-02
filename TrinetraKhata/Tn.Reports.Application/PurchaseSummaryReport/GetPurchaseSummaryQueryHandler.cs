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

namespace TN.Reports.Application.PurchaseSummaryReport
{
    public  class GetPurchaseSummaryQueryHandler:IRequestHandler<GetPurchaseSummaryQuery,Result<PagedResult<GetPurchaseSummaryResponse>>>
    {
        private readonly IPurchaseReportService _service;
        private readonly IMapper _mapper;

        public GetPurchaseSummaryQueryHandler(IPurchaseReportService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetPurchaseSummaryResponse>>> Handle(GetPurchaseSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.PurchaseSummaryReport(request.PaginationRequest, request.PurchaseSummaryDtos);
                return Result<PagedResult<GetPurchaseSummaryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetPurchaseSummaryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
