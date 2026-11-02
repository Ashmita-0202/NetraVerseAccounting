

using TN.Shared.Domain.Entities.Account;

namespace TN.Account.Application.Account.Command.BillSundry
{
    public record AddBillSundryResponse
    (
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
