using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateJournalEntryDetails
{
    public record UpdateJournalDetailsCommand
    (
        
        string id,
        string journalEntryId,
        string ledgerId,
        decimal debitAmount,
        decimal creditAmount
        
    ) : IRequest<Result<UpdateJournalDetailsResponse>>;
}
