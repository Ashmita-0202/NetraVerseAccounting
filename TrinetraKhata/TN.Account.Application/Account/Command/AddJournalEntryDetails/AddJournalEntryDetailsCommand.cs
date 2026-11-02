using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddJournalEntryDetails
{
    public record AddJournalEntryDetailsCommand
    (
            string ledgerId,
            decimal debitAmount,
            decimal creditAmount
        ) : IRequest<Result<AddJournalEntryDetailsResponse>>;
}
