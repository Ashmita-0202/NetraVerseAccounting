using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.ChartOfAccounts;
using TN.Account.Application.Account.Queries.LedgerByLedgerGroupId;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.ServiceInterface
{
    public interface IChartAccountServices
    {
        Task<Result<List<ChartsOfAccountsQueryResponse>>> GetFullChartAsync();
    }
}
