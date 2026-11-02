using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup;
using TN.Account.Application.Account.Queries.FilterLedgerGroup;
using TN.Account.Application.Account.Queries.GetLedgerGroupByMasterId;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.Account.Queries.LedgerGroupById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface ILedgerGroupService
    {
        Task<Result<PagedResult<GetAllLedgerGroupByQueryResponse>>> GetAllLedgerGroup(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetLedgerGroupByIdResponse>> GetLedgerGroupById(string ledgerGroupId, CancellationToken cancellationToken = default);

        Task<Result<List<GetLedgerGroupByMasterIdResponse>>> GetLedgerGroupByMasterId(string masterId, CancellationToken cancellationToken = default);
        Task<Result<AddLedgerGroupResponse>> Add(AddLedgerGroupCommand addLedgerGroupCommand);
        Task<Result<UpdateLedgerGroupResponse>> Update(string ledgerGroupId, UpdateLedgerGroupCommand updateLedgerGroupCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);

        Task<Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>> GetFilterLedger(CancellationToken cancellationToken);
        Task<Result<PagedResult<GetFilterLedgerGroupQueryResponse>>> GetFilterLedgerGroup(PaginationRequest paginationRequest, FilterLedgerGroupDto filterLedgerGroupDto);


    }

}
