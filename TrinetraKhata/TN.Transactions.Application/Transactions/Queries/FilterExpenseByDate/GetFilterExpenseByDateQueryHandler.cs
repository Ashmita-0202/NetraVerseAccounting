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


namespace TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate
{
    public class GetFilterExpenseByDateQueryHandler:IRequestHandler<GetFilterExpenseByDateQuery, Result<PagedResult<GetFilterExpenseByDateQueryResponse>>>
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public GetFilterExpenseByDateQueryHandler(IExpenseService expenseService,IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetFilterExpenseByDateQueryResponse>>> Handle(GetFilterExpenseByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterExpense = await _expenseService.GetFilterExpense(request.PaginationRequest,request.FilterExpenseDto);

                if (!filterExpense.IsSuccess || filterExpense.Data == null)
                {
                    return Result<PagedResult<GetFilterExpenseByDateQueryResponse>>.Failure(filterExpense.Message);
                }

                var filterExpenseResult = _mapper.Map<PagedResult<GetFilterExpenseByDateQueryResponse>>(filterExpense.Data);

                return Result<PagedResult<GetFilterExpenseByDateQueryResponse>>.Success(filterExpenseResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterExpenseByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching expense  by date: {ex.Message}");
            }
        }
    }
}
