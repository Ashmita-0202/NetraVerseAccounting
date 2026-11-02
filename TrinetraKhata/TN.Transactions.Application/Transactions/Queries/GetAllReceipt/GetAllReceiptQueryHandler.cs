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


namespace TN.Transactions.Application.Transactions.Queries.GetAllReceipt
{
    public  class GetAllReceiptQueryHandler:IRequestHandler<GetAllReceiptQuery,Result<PagedResult<GetAllReceiptQueryResponse>>>
    {
        private readonly IReceiptServices _receiptService;
        private readonly IMapper _mapper;

        public GetAllReceiptQueryHandler(IReceiptServices receiptServices,IMapper mapper)
        {
             _receiptService= receiptServices;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<GetAllReceiptQueryResponse>>> Handle(GetAllReceiptQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allReceipt = await _receiptService.GetAll(request.PaginationRequest,request.ledgerId, cancellationToken);
                var allReceiptDisplay = _mapper.Map<PagedResult<GetAllReceiptQueryResponse>>(allReceipt.Data);

                return Result<PagedResult<GetAllReceiptQueryResponse>>.Success(allReceiptDisplay);
            }
            catch (Exception ex)
            {
               throw new Exception("an error occurred while fetching receipt", ex);
            
            }

        } 
    }
}
