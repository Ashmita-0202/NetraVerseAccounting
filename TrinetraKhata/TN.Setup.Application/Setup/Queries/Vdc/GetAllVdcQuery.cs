using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Vdc
{
   public record GetAllVdcQuery(PaginationRequest paginationRequest):IRequest<Result<PagedResult<GetAllVdcResponse>>>;
}
