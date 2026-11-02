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


namespace TN.Transactions.Application.Transactions.Queries.GetAllExpense
{
    public class GetAllExpenseQueryHandler:IRequestHandler<GetAllExpenseQuery,Result<PagedResult<GetAllExpenseQueryResponse>>>
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public GetAllExpenseQueryHandler(IExpenseService expenseService, IMapper mapper) 
        {
            _expenseService=expenseService;
            _mapper=mapper;
        
        }

        public async Task<Result<PagedResult<GetAllExpenseQueryResponse>>> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allExpense = await _expenseService.GetAll(request.PaginationRequest, cancellationToken);
                var allExpenseDisplay = _mapper.Map<PagedResult<GetAllExpenseQueryResponse>>(allExpense.Data);

                return Result<PagedResult<GetAllExpenseQueryResponse>>.Success(allExpenseDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while fetching expense", ex);

            }
        }
    }
}
