using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.Account.Queries.OpeningClosingBalance;
using TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface IOpeningClosingBalanceServices
    {
        Task<Result<PagedResult<OpeningClosingBalanceResponse>>> GetOpeningClosingBalance(string fyId,PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

        Task<Result<OpeningClosingBalanceByLedgerResponse>> GetOpeningClosingBalanceByLedger(OpeningClosingBalanceDTOs openingClosingBalanceDTOs, CancellationToken cancellationToken = default);
    }
}
