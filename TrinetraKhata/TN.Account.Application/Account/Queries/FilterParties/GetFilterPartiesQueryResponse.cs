using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterParties
{
    public record  GetFilterPartiesQueryResponse
    (
             string id,
            string name,
            DateTime createdDate,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string subledgerGroupId
    );
}
