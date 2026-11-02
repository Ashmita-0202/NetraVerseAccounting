using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.ChartOfAccounts
{
    public record ChartsOfAccountsQueryResponse
    (
        string id,
        string name,
        decimal balance,
        string balanceType,
        List<LedgerGroupResponse> ledgerGroupResponses
        );

    public record LedgerGroupResponse(
        string id,
        string name,
         decimal balance,
         string balanceType,
        List<SubLedgerGroupResponse>  SubLedgerGroupResponses
        );

    public record SubLedgerGroupResponse(
        string id,
        string name,
         decimal balance,
         string balanceType,
        List<LedgerResponse> ledgerResponses
        );

    public record LedgerResponse
        (
        string id,
        string name,
         decimal balance,
         string balanceType
        );
}
