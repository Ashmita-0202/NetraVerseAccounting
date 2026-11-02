using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Queries.GetIncomeById
{
    public  class GetIncomeByIdQueryHandler:IRequestHandler<GetIncomeByIdQuery, Result<GetIncomeByIdQueryResponse>>
    {
       
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public GetIncomeByIdQueryHandler(IIncomeService incomeService,IMapper mapper) 
        {
            _incomeService = incomeService;
            _mapper=mapper;
        
        }

        public async Task<Result<GetIncomeByIdQueryResponse>> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var income = await _incomeService.GetIncomeById(request.id);

                return Result<GetIncomeByIdQueryResponse>.Success(income.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching income by Id", ex);

            }
        }
    }
}
