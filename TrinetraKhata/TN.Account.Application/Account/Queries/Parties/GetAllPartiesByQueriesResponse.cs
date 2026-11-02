using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.Parties
{
    public record GetAllPartiesByQueriesResponse
    (
          string id = "",
            string name = "",
            string createdDate = "",
            bool? isInventoryAffected = false,
            string? address = "",
            string? panNo = "",
            string? phoneNumber = "",
            string? maxCreditPeriod = "",
            string? maxDuePeriod = "",
            string subledgerGroupId = "",
            decimal? openingBalance = null,
            bool? isdebit = false
        );
}
