using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Reports.Application.Parties_Statements.Queries
{
    public record PartyStatementQuery
    (
        string partyId
    ): IRequest<Result<List<PartyStatementQueryResponse>>>;
    
}
