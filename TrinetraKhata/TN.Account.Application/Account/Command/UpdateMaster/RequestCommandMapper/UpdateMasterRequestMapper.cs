using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.UpdateLedger;

namespace TN.Account.Application.Account.Command.UpdateMaster.RequestCommandMapper
{
  public static class UpdateMasterRequestMapper
    {
        public static UpdateMasterCommand ToCommand(this UpdateMasterRequest request, string id)
        {
            return new UpdateMasterCommand(id, request.name);
        }
    }
}
