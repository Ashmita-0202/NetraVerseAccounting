using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate
{
    public record  FilterConversionFactorByDateQuery
    (PaginationRequest PaginationRequest,FilterConversionFactorDTOs FilterConversionFactorDTOs):IRequest<Result<PagedResult<FilterConversionFactorByDateQueryResponse>>>;
}
