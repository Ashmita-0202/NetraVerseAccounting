using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.LedgerById
{
    public record class GetLedgerByIdQueryResponse
        (
            string id="",
            string name="",
            string createdDate = "",
            bool? isInventoryAffected=true,
            string? address="",
            string? panNo = "",
            string? phoneNumber="",
            string? maxCreditPeriod = "",
            string? maxDuePeriod="",
            string subledgerGroupId = "",
            decimal? openingBalance = null,
            bool? isSeeded = false
        );
}
