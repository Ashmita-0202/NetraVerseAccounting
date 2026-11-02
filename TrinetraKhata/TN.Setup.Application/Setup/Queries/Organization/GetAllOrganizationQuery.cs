using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Organization
{
    public record  GetAllOrganizationQuery(PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<GetAllOrganizationResponse>>>;
    
}
