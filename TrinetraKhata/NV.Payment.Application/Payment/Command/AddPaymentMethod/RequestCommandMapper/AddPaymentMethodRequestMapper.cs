using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NV.Payment.Application.Payment.Command.AddPayment.RequestCommandMapper
{
    public static class AddPaymentMethodRequestMapper
    {
        public static AddPaymentMethodCommand ToCommand(this AddPaymentMethodRequest request) 
        {
            return new AddPaymentMethodCommand
                ( 
                    request.name,
                    request.subLedgerGroupsId,
                    //request.type,
                    request.isChequeNo,
                    request.isBankName,
                    request.isAccountName,
                    request.isChequeDate,
                    request.isCardNumber,
                    request.isCardHolderName,
                    request.isExpiryDate

                );
        }
    }
}
