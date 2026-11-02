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


namespace TN.Transactions.Application.Transactions.Queries.GetAllIncome
{
    public class GetAllIncomeQueryHandler:IRequestHandler<GetAllIncomeQuery,Result<PagedResult<GetAllIncomeQueryResponse>>>
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public GetAllIncomeQueryHandler(IIncomeService incomeService,IMapper mapper) 
        {
            _incomeService= incomeService;
            _mapper= mapper;
            
        }

        public async Task<Result<PagedResult<GetAllIncomeQueryResponse>>> Handle(GetAllIncomeQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var allIncome = await _incomeService.GetAll(request.PaginationRequest, cancellationToken);
                var allIncomeDisplay = _mapper.Map<PagedResult<GetAllIncomeQueryResponse>>(allIncome.Data);

                return Result<PagedResult<GetAllIncomeQueryResponse>>.Success(allIncomeDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while fetching income", ex);

            }
        }
    }
}
