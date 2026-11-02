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

namespace TN.Reports.Application.ItemwisePurchaseByExpireDate
{
    public class  GetItemwisePurchaseExpireDateQueryHandler:IRequestHandler<GetItemwisePurchaseExpireDateQuery,Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>>
    {
        private readonly IPurchaseReportService _service;
        private readonly IMapper _mapper;

        public GetItemwisePurchaseExpireDateQueryHandler(IPurchaseReportService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>> Handle(GetItemwisePurchaseExpireDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetItemwisePurchaseByExpireDate(request.PaginationRequest, request.ItemwisePurchaseExpireDateDtos);
                return Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
