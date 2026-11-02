using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup
{
    public record UpdateSubledgerGroupRequest
    (
         
            string name,
            string ledgerGroupId

    );
}
