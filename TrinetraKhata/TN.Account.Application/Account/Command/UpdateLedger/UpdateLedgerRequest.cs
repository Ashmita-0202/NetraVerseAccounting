using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateLedger
{
    public record UpdateLedgerRequest
    (

        string name,
         bool? isInventoryAffected,
         string? address,
         string? panNo,
         string? phoneNumber,
         string? maxCreditPeriod,
         string? maxDuePeriod,
         string subledgerGroupId,
         string? vat,
        string? discount
    );
}
