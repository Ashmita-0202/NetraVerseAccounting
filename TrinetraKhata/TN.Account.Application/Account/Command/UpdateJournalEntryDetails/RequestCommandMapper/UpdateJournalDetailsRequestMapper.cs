using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateJournalEntryDetails.RequestCommandMapper
{
  public static class UpdateJournalDetailsRequestMapper
    {
        public static UpdateJournalDetailsCommand ToCommand(this UpdateJournalDetailsRequest request, string id) 
        {
            return new UpdateJournalDetailsCommand
                (
                    id,
                    request.journalEntryId,
                    request.ledgerId,
                    request.debitAmount,
                    request.creditAmount
                );

        }
    }
}
