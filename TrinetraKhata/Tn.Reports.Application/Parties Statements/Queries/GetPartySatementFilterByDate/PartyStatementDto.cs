using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate
{
    public record  PartyStatementDto
    (
         string partyId,
         string? startDate,
        string? endDate

    );
}
