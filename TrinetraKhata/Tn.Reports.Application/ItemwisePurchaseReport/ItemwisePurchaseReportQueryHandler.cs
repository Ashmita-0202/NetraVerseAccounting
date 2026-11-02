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

namespace TN.Reports.Application.ItemwisePurchaseReport
{
    public class ItemwisePurchaseReportQueryHandler:IRequestHandler<ItemwisePurchaseReportQuery,Result<PagedResult<ItemwisePurchaseReportQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public ItemwisePurchaseReportQueryHandler(IPurchaseReportService purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<ItemwisePurchaseReportQueryResponse>>> Handle(ItemwisePurchaseReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _purchaseReportService.GetItemwisePurchaseReport(request.PaginationRequest, request.ItemwisePurchaseReportDto);
                return Result<PagedResult<ItemwisePurchaseReportQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ItemwisePurchaseReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
