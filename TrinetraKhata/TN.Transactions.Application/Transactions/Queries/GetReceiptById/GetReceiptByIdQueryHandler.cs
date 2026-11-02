using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Queries.GetReceiptById
{
    public class GetReceiptByIdQueryHandler:IRequestHandler<GetReceiptByIdQuery, Result<GetReceiptByIdQueryResponse>>
    {
        private readonly IReceiptServices _receiptServices;
        private readonly IMapper _mapper;

        public GetReceiptByIdQueryHandler(IReceiptServices receiptServices,IMapper mapper)
        {
            _receiptServices=receiptServices;
            _mapper=mapper;
        
        }

        public async Task<Result<GetReceiptByIdQueryResponse>> Handle(GetReceiptByIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                var receiptById = await _receiptServices.GetReceiptById(request.id, cancellationToken);
                return Result<GetReceiptByIdQueryResponse>.Success(receiptById.Data);
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Receipt by using Id",ex);
            
            }
        }
    }
}
