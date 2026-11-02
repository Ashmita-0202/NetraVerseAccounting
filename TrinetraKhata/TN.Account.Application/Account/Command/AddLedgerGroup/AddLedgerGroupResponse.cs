using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddLedgerGroup
{
    public record AddLedgerGroupResponse
    (string name,
            bool isCustom = true,
            string masterId="",
            bool isPrimary = true
        );
}
