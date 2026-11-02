using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.AccountPayable
{
    public record ARAPWithTotals
    (
         PagedResult<AccountPayableQueryResponse> PagedItems,
            decimal? TotalReceivableAmount,
            decimal? TotalPayableAmount
        );
}
