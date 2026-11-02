using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate
{
    public record  GetFilterIncomeByDateQuery
    (
        PaginationRequest PaginationRequest,FilterIncomeDto FilterIncomeDto
    ):IRequest<Result<PagedResult<GetFilterIncomeByDateQueryResponse>>>;
}
