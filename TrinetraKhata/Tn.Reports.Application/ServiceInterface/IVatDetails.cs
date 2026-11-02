using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.TrialBalance;
using TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IVatDetails
    {
        Task<Result<PagedResult<PurchaseAndSalesVATQueryResponse>>> GetVATReport(PurchaseAndSalesVATQueries request, CancellationToken cancellationToken = default);
    }
}
