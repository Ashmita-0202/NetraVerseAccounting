using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Transactions.Application.Transactions.Queries.GetAllReceipt
{
    public record  GetAllReceiptQuery
   (PaginationRequest PaginationRequest, string? ledgerId):IRequest<Result<PagedResult<GetAllReceiptQueryResponse>>>;
}
