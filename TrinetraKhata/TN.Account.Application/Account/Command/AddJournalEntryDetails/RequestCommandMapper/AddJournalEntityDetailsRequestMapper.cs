using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddJournalEntryDetails.RequestCommandMapper
{
    public static class AddJournalEntityDetailsRequestMapper
    {
        public static AddJournalEntryDetailsCommand ToCommand(this AddJournalEntryDetailsRequest request)
        {
           return new AddJournalEntryDetailsCommand
                (
                    request.ledgerId,
                    request.debitAmount,
                    request.creditAmount
               );

        }
    }
}
