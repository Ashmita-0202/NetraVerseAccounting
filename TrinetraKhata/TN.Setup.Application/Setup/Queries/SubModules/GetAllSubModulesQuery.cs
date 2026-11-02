using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.SubModules
{
    public record  GetAllSubModulesQuery(PaginationRequest PaginationRequest): IRequest<Result<PagedResult<GetAllSubModulesResponse>>>;

}
