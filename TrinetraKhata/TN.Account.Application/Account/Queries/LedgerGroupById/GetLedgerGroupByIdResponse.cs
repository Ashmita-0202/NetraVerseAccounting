using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.LedgerGroupById
{
    public record class GetLedgerGroupByIdResponse(string id,
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary,
            bool isSeeded);
}
