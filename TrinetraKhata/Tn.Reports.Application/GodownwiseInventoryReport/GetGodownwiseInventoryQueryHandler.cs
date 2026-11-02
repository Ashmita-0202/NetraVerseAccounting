using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.ItemRateHistory;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.GodownwiseInventoryReport
{
    public class GetGodownwiseInventoryQueryHandler : IRequestHandler<GetGodownwiseInventoryQuery, Result<PagedResult<GetGodownwiseInventoryQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseReportService _service;

        public GetGodownwiseInventoryQueryHandler(IPurchaseReportService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Result<PagedResult<GetGodownwiseInventoryQueryResponse>>> Handle(GetGodownwiseInventoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetGodownwiseInventory(request.PaginationRequest, request.godownwiseInventoryDtos, cancellationToken);
                return Result<PagedResult<GetGodownwiseInventoryQueryResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetGodownwiseInventoryQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
