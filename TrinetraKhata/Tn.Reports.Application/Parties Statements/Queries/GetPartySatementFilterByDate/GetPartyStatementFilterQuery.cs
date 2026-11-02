using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilter
{
    public record  GetPartyStatementFilterQuery
   (PaginationRequest PaginationRequest,PartyStatementDto PartyStatementDto):IRequest<Result<PagedResult<GetPartyStatementFilterResponse>>>;
}
