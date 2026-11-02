using MediatR;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Province
{
    public record GetAllProvinceQuery(PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<GetAllProvinceResponse>>>;
   
}

