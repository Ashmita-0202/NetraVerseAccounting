using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.PurchaseReport
{
    public record  GetPurchaseReportQuery
   (PaginationRequest PaginationRequest, PurchaseReportDtos purchaseReportDtos) : IRequest<Result<PagedResult<GetPurchaseReportQueryResponse>>>;
}
