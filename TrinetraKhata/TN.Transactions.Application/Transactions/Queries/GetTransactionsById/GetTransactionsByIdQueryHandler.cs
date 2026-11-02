using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Queries.GetTransactionsById
{
    public  class GetTransactionsByIdQueryHandler: IRequestHandler<GetTransactionsByIdQuery,Result<GetTransactionsByIdQueryResponse>>
    {
        private readonly ITransactionsService _service;
        private readonly IMapper _mapper;

        public GetTransactionsByIdQueryHandler(ITransactionsService service,IMapper mapper) 
        {
            _service=service;
            _mapper=mapper;
        }

        public async Task<Result<GetTransactionsByIdQueryResponse>> Handle(GetTransactionsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var transactionsById = await _service.GetTransactionsById(request.id);
                return Result<GetTransactionsByIdQueryResponse>.Success(transactionsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Transactions by using id", ex);
            }
        }
    }
}
