using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear
{
    public record GetSelectedFiscalYearQuery
    (
        PaginationRequest PaginationRequest
        ) : IRequest<Result<List<GetSelectedFiscalYearQueryResponse>>>;
}
