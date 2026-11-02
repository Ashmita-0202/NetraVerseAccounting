using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterParties
{
    public record  GetFilterPartiesQuery
   (
        PaginationRequest PaginationRequest,
        FilterPartiesDto FilterPartiesDto
   ):IRequest<Result<PagedResult<GetFilterPartiesQueryResponse>>>;
}
