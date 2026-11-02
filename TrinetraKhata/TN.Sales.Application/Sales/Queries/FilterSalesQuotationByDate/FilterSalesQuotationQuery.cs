using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate
{
    public record FilterSalesQuotationQuery
    (PaginationRequest PaginationRequest, FilterSalesDetailsDTOs FilterSalesDetailsDTOs) : IRequest<Result<PagedResult<FilterSalesQuotationQueryResponse>>>;
}
