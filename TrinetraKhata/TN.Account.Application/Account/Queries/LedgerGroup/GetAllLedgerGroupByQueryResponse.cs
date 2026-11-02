using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.LedgerGroup
{
    public record class GetAllLedgerGroupByQueryResponse(
            string id,
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary,
            bool? isSeeded
        );
}
