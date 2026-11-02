using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddJournalEntryDetails
{
    public record AddJournalEntryDetailsRequest
    (
            string ledgerId,
            decimal debitAmount,
            decimal creditAmount
        );
}
