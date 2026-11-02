using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.ImportExcelForLedgers;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Queries.AccountPayable;
using TN.Account.Application.Account.Queries.AccountReceivable;
using TN.Account.Application.Account.Queries.ARAPByLedgerId;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Account.Application.Account.Queries.GetBalance;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.Account.Queries.LedgerByLedgerGroupId;
using TN.Account.Application.Account.Queries.Parties;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface ILedgerService
    {
        Task<Result<GetBalanceByQueryResponse>> GetBalance(string ledgerId);

        Task<Result<ARAPWithTotals>> GetAccountPayable(PaginationRequest paginationRequest, string ledgerId);

        Task<Result<ArApByLedgerQueryResponse>> GetArApByLedger(string ledgerId, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<AccountReceivableQueryResponse>>> GetAccountReceivable(PaginationRequest paginationRequest,string? ledgerId);
        Task<Result<PagedResult<GetAllLedgerByQueryResponse>>> GetAllLedger(PaginationRequest paginationRequest,CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetAllPartiesByQueriesResponse>>> GetAllParties(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetFilterPartiesQueryResponse>>> GetFilterParties(PaginationRequest paginationRequest, FilterPartiesDto filterPartiesDto);
        Task<Result<GetLedgerByIdQueryResponse>> GetLedgerById(string ledgerId, CancellationToken cancellationToken = default);

        Task<Result<AddLedgerResponse>> Add(AddLedgerCommand addLedgerCommand);
        Task<Result<UpdateLedgerResponse>> Update(string ledgerId, UpdateLedgerCommand updateLedgerCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);

        Task<Result<List<GetAllLedgerByLedgerGroupIdResponse>>> GetLedgerByLedgerGroupId(string ledgerGroupId, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetFilterLedgerByResponse>>> GetFilterLedger(PaginationRequest paginationRequest, FilterLedgerDto filterLedgerDto); 
    

        Task<Result<LedgerExcelResponse>> AddLedgerExcel(IFormFile formFile, CancellationToken cancellationToken = default);
    }
}
