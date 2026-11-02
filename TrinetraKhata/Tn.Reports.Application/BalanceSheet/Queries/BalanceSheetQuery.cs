using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record BalanceSheetQuery
    (PaginationRequest PaginationRequest, string? CompanyId) : IRequest<Result<PagedResult<BalanceSheetFinalResponse>>>;
}
