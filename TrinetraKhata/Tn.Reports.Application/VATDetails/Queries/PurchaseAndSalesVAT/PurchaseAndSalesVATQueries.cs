using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT
{
    public record PurchaseAndSalesVATQueries
    (
        PaginationRequest PaginationRequest, string? companyId
        ): IRequest<Result<PagedResult<PurchaseAndSalesVATQueryResponse>>>;
}
