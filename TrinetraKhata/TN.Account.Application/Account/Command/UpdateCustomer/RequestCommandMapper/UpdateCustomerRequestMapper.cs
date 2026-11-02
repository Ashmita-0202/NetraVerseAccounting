using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.UpdateLedger;

namespace TN.Account.Application.Account.Command.UpdateCustomer.RequestCommandMapper
{
    public static class UpdateCustomerRequestMapper
    {
        public static UpdateCustomerCommand ToCommand(this UpdateCustomerRequest request, string id)
        {
            return new UpdateCustomerCommand
                (
                     id,
                    request.fullName,
                    request.address,
                    request.contact,
                    request.email,
                    request.description,
                    request.panNo,
                    request.maxDueDates,
                    request.maxCreditLimit,
                    request.isEnabled,
                    request.openingBalance,
                    request.balanceType,
                    request.isSmsEnabled,
                    request.isEmailEnabled,
                    request.ledgerId
                
                
                );
        }
    }
}
