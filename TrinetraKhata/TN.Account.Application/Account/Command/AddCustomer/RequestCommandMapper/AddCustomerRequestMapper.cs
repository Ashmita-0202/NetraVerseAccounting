using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddLedger;

namespace TN.Account.Application.Account.Command.AddCustomer.RequestCommandMapper
{
    public static class AddCustomerRequestMapper
    {
        public static AddCustomerCommand ToCommand(this AddCustomerRequest request)
        {
            return new AddCustomerCommand
                (
                   
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
