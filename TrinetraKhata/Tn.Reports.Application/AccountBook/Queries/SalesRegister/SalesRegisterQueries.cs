using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.SalesRegister
{
    public record SalesRegisterQueries
    (
       PaginationRequest PaginationRequest, SalesRegisterDTOs SalesRegisterDTOs
        ) : IRequest<Result<PagedResult<SalesRegisterQueryResponse>>>;
}
