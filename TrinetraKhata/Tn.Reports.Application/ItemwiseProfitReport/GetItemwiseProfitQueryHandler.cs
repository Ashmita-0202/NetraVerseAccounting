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

namespace TN.Reports.Application.ItemwiseProfitReport
{
    public class GetItemwiseProfitQueryHandler:IRequestHandler<GetItemwiseProfitQuery,Result<PagedResult<GetItemwiseProfitQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public GetItemwiseProfitQueryHandler(IPurchaseReportService  purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetItemwiseProfitQueryResponse>>> Handle(GetItemwiseProfitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _purchaseReportService.GetItemwiseProfit(request.PaginationRequest, request.itemwiseProfitDtos);
                return Result<PagedResult<GetItemwiseProfitQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetItemwiseProfitQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
