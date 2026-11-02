using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;

namespace TN.Reports.Application.AccountBook.Queries.PurchaseRegister
{
    public record PurchaseRegisterQueryResponse
    (
        string date,
        string billNumber,
        string accountId,
        string type,
        decimal totalAmount,
        string companyId,
        decimal? vatPercent,
        decimal? vatAmount,
        string? referenceNumber,
         string ledgerId,

        List<PurchaseRegisterItemsDTOs> purchaseRegisterItems
        );

    public record PurchaseRegisterItemsDTOs
        (
           decimal quantity,
            string unitId,
            string itemId,
            decimal amount
        );
}
