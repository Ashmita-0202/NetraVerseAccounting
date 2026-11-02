using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterSundryBill
{
    public record  FilterSundryBillQuery
    (PaginationRequest PaginationRequest,FilterSundryBillDto FilterSundryBillDto):IRequest<Result<PagedResult<FilterSundryBillQueryResponse>>>;
}
