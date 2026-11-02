using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NV.Payment.Application.Payment.Command.UpdatePayment.RequestCommandMapper
{
    public static class UpdatePaymentMethodRequestMapper
    {
        public static UpdatePaymentMethodCommand ToCommand(this UpdatePaymentMethodRequest request, string id) 
        {
           
            return new UpdatePaymentMethodCommand
                (
                    id,
                    request.name,
                    request.subLedgerGroupsId



                );

        }
    }
}
