using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using MediatR;
using TN.Reports.Application.PurchaseReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ItemRateHistory
{
    public class GetItemRateHistoryQueryHandler:IRequestHandler<GetItemRateHistoryQuery,Result<PagedResult<GetItemRateHistoryQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public GetItemRateHistoryQueryHandler(IPurchaseReportService purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetItemRateHistoryQueryResponse>>> Handle(GetItemRateHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _purchaseReportService.GetItemRateHistory(request.PaginationRequest, request.itemRateHistoryDtos);
                return Result<PagedResult<GetItemRateHistoryQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetItemRateHistoryQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
