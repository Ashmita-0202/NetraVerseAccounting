using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetailsById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface IJournalDetailsServices
    {
        Task<Result<string>> GetCurrentJournalRefNo();
        Task<Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>> GetAllJournalDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken=default);

        Task<Result<GetJournalEntryDetailsByIdResponse>> GetJournalDetailsById(string id, CancellationToken cancellationToken = default);

        Task<Result<UpdateJournalDetailsResponse>> Update(string id, UpdateJournalDetailsCommand updateJournalDetailsCommand);

        Task<Result<AddJournalEntryDetailsResponse>> Add(AddJournalEntryDetailsCommand addJournalEntryDetailsCommand);
    }
}
