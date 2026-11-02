using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;


namespace TN.Transactions.Application.Transactions.Queries.GetExpenseById
{
    public class GetExpenseByIdQueryHandler:IRequestHandler<GetExpenseByIdQuery,Result<GetExpenseByIdQueryResponse>>
    {
        private readonly IExpenseService _expenseService;

        public GetExpenseByIdQueryHandler(IExpenseService expenseService)
        {
            _expenseService= expenseService;
          
        }

        public async Task<Result<GetExpenseByIdQueryResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var expense = await _expenseService.GetExpenseById(request.id);

                return Result<GetExpenseByIdQueryResponse>.Success(expense.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching expense by Id", ex);

            }
        }
    }
}
