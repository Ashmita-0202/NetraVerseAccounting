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


namespace TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate
{
    public class  GetFilterIncomeByDateQueryHandler:IRequestHandler<GetFilterIncomeByDateQuery,Result<PagedResult<GetFilterIncomeByDateQueryResponse>>>
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public GetFilterIncomeByDateQueryHandler(IIncomeService incomeService,IMapper mapper)
        {
            _incomeService=incomeService;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetFilterIncomeByDateQueryResponse>>> Handle(GetFilterIncomeByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterIncome = await _incomeService.GetIncomeFilter(request.PaginationRequest,request.FilterIncomeDto);

                if (!filterIncome.IsSuccess || filterIncome.Data == null)
                {
                    return Result<PagedResult<GetFilterIncomeByDateQueryResponse>>.Failure(filterIncome.Message);
                }

                var filterIncomeResult = _mapper.Map<PagedResult<GetFilterIncomeByDateQueryResponse>>(filterIncome.Data);

                return Result<PagedResult<GetFilterIncomeByDateQueryResponse>>.Success(filterIncomeResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterIncomeByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching income  by date: {ex.Message}");
            }
        }
    }
}
