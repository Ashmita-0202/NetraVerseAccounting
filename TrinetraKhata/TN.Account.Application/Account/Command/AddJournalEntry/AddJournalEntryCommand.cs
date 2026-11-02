using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddJournalEntry
{
    public record AddJournalEntryCommand
    (
        string? referenceNumber,
        string? transactionDate,
        string description,
        List<AddJournalEntryDetailsRequest> AddJournalEntryDetails
        ) : IRequest<Result<AddJournalEntryResponse>>;
}
