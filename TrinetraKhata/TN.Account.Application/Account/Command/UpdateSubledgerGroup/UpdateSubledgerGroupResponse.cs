using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup
{
    public record  UpdateSubledgerGroupResponse
    (
             
            string name,
            string ledgerGroupId

   );
}
