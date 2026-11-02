using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NV.Payment.Application.Payment.Queries.GetPaymentMethod;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.Payment.Queries.GetPayment
{
    public record GetAllPaymentMethodQuery
    (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllPaymentMethodQueryResponse>>>;
}
