using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Command.UpdateMaster;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Account.Application.Account.Queries.master;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface IMasterService
    {
        Task<Result<PagedResult<GetAllMasterByQueryResponse>>> GetAllMaster(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetMasterByIdQueryResponse>> GetMasterById(string masterId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken);
        Task<Result<UpdateMasterResponse>> Update(string id, UpdateMasterCommand updateMasterCommand);
    }
}
