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

namespace TN.Reports.Application.PurchaseReturnReport
{
    public  class GetPurchaseReturnReportQueryHandler: IRequestHandler<GetPurchaseReturnReportQuery, Result<PagedResult<GetPurchaseReturnReportQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public GetPurchaseReturnReportQueryHandler(IPurchaseReportService purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetPurchaseReturnReportQueryResponse>>> Handle(GetPurchaseReturnReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _purchaseReportService.GetPurchaseReturnReport(request.PaginationRequest, request.PurchaseReturnReportDto);
                return Result<PagedResult<GetPurchaseReturnReportQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetPurchaseReturnReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
