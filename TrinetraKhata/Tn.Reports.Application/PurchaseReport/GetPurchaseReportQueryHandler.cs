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

namespace TN.Reports.Application.PurchaseReport
{
    public class GetPurchaseReportQueryHandler:IRequestHandler<GetPurchaseReportQuery,Result<PagedResult<GetPurchaseReportQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseReportService _service;

        public GetPurchaseReportQueryHandler(IPurchaseReportService service,IMapper mapper) 
        {
            _mapper=mapper;
            _service=service;
        
        }

        public async Task<Result<PagedResult<GetPurchaseReportQueryResponse>>> Handle(GetPurchaseReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetAllPurchaseReport(request.PaginationRequest, request.purchaseReportDtos);
                return Result<PagedResult<GetPurchaseReportQueryResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetPurchaseReportQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
