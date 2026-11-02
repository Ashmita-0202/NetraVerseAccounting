using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate
{
    public record  GetFilterSubledgerGroupQueryResponse
    (
         string id,
        string name,
        string ledgerGroupId,
        bool? isSeeded
    );
}
