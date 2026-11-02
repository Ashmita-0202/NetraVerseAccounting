using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.SubledgerGroup
{
    public record  GetAllSubledgerGroupQueryResposne
    (
        string id,
        string name,
        string ledgerGroupId,
        bool? isSeeded

    );
}
