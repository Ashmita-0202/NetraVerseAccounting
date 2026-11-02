using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.LedgerBalanceDTOs
{
    public record MasterDTOs
    (
        string Id,
        string Name
    );

    public record LedgerGroupDTOs
    (
        string Id,
        string Name,
        MasterDTOs Master
    );

    public record SubLedgerGroupDTOs
    (
        string Id,
        string Name,
        LedgerGroupDTOs LedgerGroup
    );

    public record LedgersDTOs
    (
        string Id,
        string Name,
        SubLedgerGroupDTOs SubLedgerGroup
    );
}
