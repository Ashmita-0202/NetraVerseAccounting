using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddSubledgerGroup
{
    public record AddSubledgerGroupResponse
    (
            string id,
            string name,
            string ledgerGroupId
    );
}
