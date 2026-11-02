using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.Payment.Queries.FilterPaymentMethod
{
    public record  GetFilterPaymentMethodQuery
    (
        
        PaginationRequest PaginationRequest,
        FilterPaymentMethodDto FilterPaymentMethodDto
        
    ):IRequest<Result<PagedResult<GetFilterPaymentMethodResponse>>>;
}
