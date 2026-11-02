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

namespace TN.Transactions.Application.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsByQueryHandler:IRequestHandler<GetAllTransactionsByQuery, Result<PagedResult<GetAllTransactionsByQueryResponse>>>
    {
        private readonly ITransactionsService _service;
        private readonly IMapper _mapper;

        public GetAllTransactionsByQueryHandler(ITransactionsService service,IMapper mapper)
        {
            _service=service;
            _mapper=mapper;
        
        }

        public  async Task<Result<PagedResult<GetAllTransactionsByQueryResponse>>> Handle(GetAllTransactionsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allTransactions = await _service.GetAllTransactions(request.PaginationRequest, cancellationToken);
                var allTransactionsDisplay = _mapper.Map<PagedResult<GetAllTransactionsByQueryResponse>>(allTransactions.Data);

                return Result<PagedResult<GetAllTransactionsByQueryResponse>>.Success(allTransactionsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all Transactions", ex);
            }
        }
    }
}
