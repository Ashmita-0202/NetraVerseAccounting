using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddSubledgerGroup;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup;
using TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate;
using TN.Account.Application.Account.Queries.SubledgerGroup;
using TN.Account.Application.Account.Queries.SubledgerGroupById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface ISubledgerGroupService
    {
        Task<Result<AddSubledgerGroupResponse>> Add(AddSubledgerGroupCommand command);
        Task<Result<PagedResult<GetAllSubledgerGroupQueryResposne>>> GetAll(PaginationRequest paginationRequest,
            CancellationToken cancellationToken = default);
        Task<Result<GetSubledgerGroupByIdResponse>> GetById(string id, CancellationToken cancellationToken = default);
       Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateSubledgerGroupResponse>> Update(string id, UpdateSubledgerGroupCommand command);

        Task<Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>> GetFilterSubLedgerGroup(PaginationRequest paginationRequest, FilterSubledgerGroupDto filterSubledgerGroupDto, CancellationToken cancellationToken = default);
    }
}
