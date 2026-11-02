using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterLedgerGroup
{
    public record  GetFilterLedgerGroupQueryResponse
    (
            string id,
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary,
             DateTime createdAt,
             bool? isSeeded
    );
}
