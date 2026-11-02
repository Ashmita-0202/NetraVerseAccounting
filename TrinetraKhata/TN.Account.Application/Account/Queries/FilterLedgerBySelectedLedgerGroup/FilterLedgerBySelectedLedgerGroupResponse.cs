using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup
{
    public record FilterLedgerBySelectedLedgerGroupResponse
   (
        string id,
        string name,
        string ledgerGroupId
            
        );
}
