using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger
{
    public record OpeningClosingBalanceDTOs
    (
        string ledgerId,    
        string fyId
        );
}
