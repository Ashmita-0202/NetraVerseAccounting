using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Annex13.Queries
{
    public record AnnexReportQuery
    (
        PaginationRequest PaginationRequest,
        AnnexReportDTOs AnnexReportDTOs
        ) : IRequest<Result<PagedResult<AnnexReportQueryResponse>>>;
}
