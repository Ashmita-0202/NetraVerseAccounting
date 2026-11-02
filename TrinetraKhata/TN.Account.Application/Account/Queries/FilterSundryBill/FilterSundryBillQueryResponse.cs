using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Account;

namespace TN.Account.Application.Account.Queries.FilterSundryBill
{
    public record  FilterSundryBillQueryResponse
    (
          string id,

        string name,
        BillSundryType billType,
        decimal? defaultValue,

        BillSundryNature BillSundryNature,
        bool isCOGSAffected,
        bool isCOGPAffected,
        bool isCOGSTAffected,
        bool isSalesAccountingAffected,
        bool isPurchaseAccountingAffected,
        bool isSalesAmountAdjusted,
        bool isPurchaseAmountAdjusted,
        bool CustomerAmountAdjusted,
        bool VendorAmountAdjusted,
         string? salesAdjustedLedgerId,
        string? customerAdjustedLedgerId,
        string? purchaseAdjustedLedgerId,
        string? vendorAdjustedLedgerId,
        CalculationType calculationType,
        CalculationTypeOf calculationTypeOf

    );
}
