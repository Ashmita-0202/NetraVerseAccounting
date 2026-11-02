using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Queries.FilterJournalByDate;
using TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup;
using TN.Account.Application.Account.Queries.JournalEntry;
using TN.Account.Application.Account.Queries.JournalEntryById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface IJournalServices
    {
        Task<Result<AddJournalEntryResponse>> Add(AddJournalEntryCommand request);
        Task<Result<AddJournalEntryDetailsResponse>> AddJournalDetails(AddJournalEntryDetailsCommand request);
        Task<Result<GetJournalEntryByIdResponse>> GetJournalById(string id, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetAllJournalEntryByQueryResponse>>> GetAllJournalEntriesAsync(PaginationRequest paginationRequest);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateJournalEntryResponse>> Update(string id, UpdateJournalEntryCommand updateJournalEntryCommand);

        Task<Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>> GetJournalFilter(PaginationRequest paginationRequest,FilterJournalDTOs filterJournalDTOs);

    
}
}


