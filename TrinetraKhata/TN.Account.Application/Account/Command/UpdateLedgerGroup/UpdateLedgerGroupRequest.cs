using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateLedgerGroup
{
    public record UpdateLedgerGroupRequest
    (
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary
    );
}
