using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate
{
    public class GetFilterReceiptQueryHandler:IRequestHandler<GetFilterReceiptQuery, Result<PagedResult<GetFilterReceiptQueryRespopnse>>>
    {
        private readonly IReceiptServices _receiptServices;
        private readonly IMapper _mapper;

        public GetFilterReceiptQueryHandler(IReceiptServices receiptServices,IMapper mapper)
        {
            _receiptServices=receiptServices;
            _mapper=mapper;
             
        }

        public async Task<Result<PagedResult<GetFilterReceiptQueryRespopnse>>> Handle(GetFilterReceiptQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterReceipt = await _receiptServices.GetFilterReceipt(request.PaginationRequest,request.FilterReceiptDto);

                if (!filterReceipt.IsSuccess || filterReceipt.Data == null)
                {
                    return Result<PagedResult<GetFilterReceiptQueryRespopnse>>.Failure(filterReceipt.Message);
                }

                var filterReceiptResult = _mapper.Map<PagedResult<GetFilterReceiptQueryRespopnse>>(filterReceipt.Data);

                return Result<PagedResult<GetFilterReceiptQueryRespopnse>>.Success(filterReceiptResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterReceiptQueryRespopnse>>.Failure(
                    $"An error occurred while fetching receipt  by date: {ex.Message}");
            }
        }
    }
}
