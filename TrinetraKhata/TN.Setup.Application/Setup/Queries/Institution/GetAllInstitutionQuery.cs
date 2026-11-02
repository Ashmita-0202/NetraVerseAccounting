using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Institution
{
   public record GetAllInstitutionQuery(PaginationRequest PaginationRequest): IRequest<Result<PagedResult<GetAllInstitutionResponse>>>;

}
