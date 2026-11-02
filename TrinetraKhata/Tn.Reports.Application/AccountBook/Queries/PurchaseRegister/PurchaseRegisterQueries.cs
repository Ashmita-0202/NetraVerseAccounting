using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.PurchaseRegister
{
    public record PurchaseRegisterQueries
    (
        PaginationRequest PaginationRequest, PurchaseRegisterDTOs PurchaseRegisterDTOs
        ) : IRequest<Result<PagedResult<PurchaseRegisterQueryResponse>>>;
}
