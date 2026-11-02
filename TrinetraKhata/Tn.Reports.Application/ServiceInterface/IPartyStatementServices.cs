using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.Parties_Statements.Queries;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IPartyStatementServices
    {
        Task<Result<List<PartyStatementQueryResponse>>> GetPartyStatement(string partyId);
        Task<Result<PagedResult<GetPartyStatementFilterResponse>>> GetPartyStatementFilter(PaginationRequest paginationRequest,PartyStatementDto partyStatementDto);
    }
}
