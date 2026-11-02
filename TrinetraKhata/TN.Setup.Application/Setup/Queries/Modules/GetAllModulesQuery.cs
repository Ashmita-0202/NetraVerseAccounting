using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Modules
{
    public record GetAllModulesQuery(PaginationRequest PaginationRequest) :IRequest<Result<PagedResult<GetAllModulesResponse>>>;
}
