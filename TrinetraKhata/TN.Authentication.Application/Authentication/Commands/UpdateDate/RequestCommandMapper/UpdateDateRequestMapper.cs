using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdateDate.RequestCommandMapper
{
    public static class UpdateDateRequestMapper
    {
        public static UpdateDateCommand ToCommand(this UpdateDateRequest request,string userId,DateTime date)
        {
            return new UpdateDateCommand(
              
                userId,
                date
              
            );
        }
    }
}
