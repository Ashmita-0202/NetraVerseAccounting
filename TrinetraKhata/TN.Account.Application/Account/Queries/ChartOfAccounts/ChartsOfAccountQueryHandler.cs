using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.ChartOfAccounts
{
    public sealed class ChartsOfAccountQueryHandle : IRequestHandler<ChartsOfAccountsQuery, Result<List<ChartsOfAccountsQueryResponse>>>
    {

        private readonly IChartAccountServices _chartAccountServices;
        private readonly IMapper _mapper;

        public ChartsOfAccountQueryHandle(IChartAccountServices chartAccountServices, IMapper mapper)
        {
            _mapper = mapper;
            _chartAccountServices = chartAccountServices;
            
        }
        public async Task<Result<List<ChartsOfAccountsQueryResponse>>> Handle(ChartsOfAccountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var accountChart = await _chartAccountServices.GetFullChartAsync();
                var accountChartDisplay = _mapper.Map<List<ChartsOfAccountsQueryResponse>>(accountChart.Data);
                return Result<List<ChartsOfAccountsQueryResponse>>.Success(accountChartDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all customer", ex);
            }
        }
    }
}
