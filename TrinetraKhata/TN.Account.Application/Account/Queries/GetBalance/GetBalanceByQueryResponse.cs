using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.GetBalance
{
    public record GetBalanceByQueryResponse
   (
        string ledgerId,
        decimal balance,
        string balanceType
        );
}
