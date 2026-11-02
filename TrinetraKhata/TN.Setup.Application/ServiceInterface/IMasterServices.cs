using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.master;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IMasterServices
    {
        Task<Result<PagedResult<GetAllMasterByQueryResponse>>> GetAllMaster(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
