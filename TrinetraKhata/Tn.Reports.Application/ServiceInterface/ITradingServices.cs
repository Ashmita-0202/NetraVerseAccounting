using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Reports.Application.TradingAccount;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface ITradingServices
    {
        Task<GetTradingAccountQueryResponse> GenerateTradingReport(string? startDate, string? endDate,string? companyId, CancellationToken cancellationToken=default);
    }
}
