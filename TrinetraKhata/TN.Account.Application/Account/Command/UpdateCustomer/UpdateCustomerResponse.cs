using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Enum;

namespace TN.Account.Application.Account.Command.UpdateCustomer
{
    public record UpdateCustomerResponse
   (        
            string fullName,
            string address,
            string contact,
            string? email,
            string? description,
            string? panNo,
            int? maxDueDates,
            decimal? maxCreditLimit,
            bool? isEnabled,
            decimal? openingBalance,
            BalanceType? balanceType,
            bool? isSmsEnabled,
            bool? isEmailEnabled,
            string ledgerId
    );
}
