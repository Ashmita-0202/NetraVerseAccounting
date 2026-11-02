using MediatR;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate
{
    public record FilterPurchaseQuotationQuery
    (
        PaginationRequest PaginationRequest, FilterPurchaseDetailsDTOs FilterPurchaseDetailsDTOs
        ) : IRequest<Result<PagedResult<FilterPurchaseQuotationQueryResponse>>>;
}
