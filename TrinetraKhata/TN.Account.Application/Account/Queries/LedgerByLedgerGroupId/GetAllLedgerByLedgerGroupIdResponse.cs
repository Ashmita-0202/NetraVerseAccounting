using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.LedgerByLedgerGroupId
{
    public record GetAllLedgerByLedgerGroupIdResponse
    (
                string id,
            string name,
            string createdDate,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string vat,
            string discount,
            string subledgerGroupId
        );
}
