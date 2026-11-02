using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterLedger
{
    public record  GetFilterLedgerByResponse
    (
            string id = "",
            string name = "",
            string? address = "",
            string? panNo = "",
            string? phoneNumber = "",
            string? maxCreditPeriod = "",
            string? maxDuePeriod = "",
            string subledgerGroupId = "",
            bool? isSeeded = false,
            decimal? balance = 0,
            string? balanceType=""
            
    );
}
